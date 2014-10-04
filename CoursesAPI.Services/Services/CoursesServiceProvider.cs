using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Models.Entities;
using CoursesAPI.Services.Helpers;
using CoursesAPI.Services.Exceptions;

namespace CoursesAPI.Services.Services
{
	public class CoursesServiceProvider
	{
		#region Database collections
		private readonly IUnitOfWork _uow;

		private readonly IRepository<CourseInstance> _courseInstances;
		private readonly IRepository<TeacherRegistration> _teacherRegistrations;
		private readonly IRepository<CourseTemplate> _courseTemplates; 
		private readonly IRepository<Person> _persons;
        private readonly IRepository<Semester> _semesters;

		private readonly IRepository<PersonRegistration> _personRegistrations;
		private readonly IRepository<ProjectGroup> _projectGroups;
		private readonly IRepository<Project> _projects;
		private readonly IRepository<Grade> _grades;
		private readonly IRepository<FinalGradeComposition> _finalGradeComps;
		#endregion Database collections

		public CoursesServiceProvider(IUnitOfWork uow)
		{
			_uow = uow;

			_courseInstances = _uow.GetRepository<CourseInstance>();
			_courseTemplates = _uow.GetRepository<CourseTemplate>();
			_teacherRegistrations = _uow.GetRepository<TeacherRegistration>();
			_persons = _uow.GetRepository<Person>();

			_personRegistrations = _uow.GetRepository<PersonRegistration>();
			_projectGroups = _uow.GetRepository<ProjectGroup>();
			_projects = _uow.GetRepository<Project>();
			_grades = _uow.GetRepository<Grade>();
			_finalGradeComps = _uow.GetRepository<FinalGradeComposition>();
		}

		#region Private helper classes
		private class ProjectGroupData{
			public int ProjectGroupID { get; set; }
			public int GradedProjectsCount { get; set; }
			public List<ProjectData> TheProjects = new List<ProjectData>();


			public ProjectGroupData(int ID, int GradedProjectsCount)
			{
				this.ProjectGroupID = ID;
				this.GradedProjectsCount = GradedProjectsCount;
			}

			public void AddProject(int? Grade, int Weight)
			{
				//This causes a NullReferenceException if Grade == null
				this.TheProjects.Add(new ProjectData { 
										Grade = Grade,
										Weight = Weight
									});
			}

			public class ProjectData
			{
				public int? Grade { get; set; }
				public int Weight { get; set; }
			}

			//Checks if all the weights are the same
			public bool validate()
			{
				int theWeight;
				if(TheProjects.Count() == 0){
					return true;
				}
				else
				{
					theWeight = TheProjects[0].Weight;
				}
				foreach(ProjectData pd in TheProjects){
					if(pd.Weight != theWeight){
						return false;
					}
				}
				return true;
			}

			public FinalGradeDTO getTotalGrade()
			{
				if(!validate()){
					throw new Exception("The weights of the projects in the project group are not the same");
				}

				List<ProjectData> usedProjects = new List<ProjectData>();

				//Find the grades that should be used
				foreach(ProjectData pd in TheProjects){
					if(usedProjects.Count < GradedProjectsCount){
						if(pd.Grade != null){
							usedProjects.Add(pd);
						}
					}
					else
					{
						if (pd.Grade != null)
						{
							//Find the lowest grade
							ProjectData lowest = pd;
							bool stillLowest = true;
							foreach (ProjectData p in usedProjects)
							{
								if (p.Grade < lowest.Grade)
								{
									lowest = p;
									stillLowest = false;
								}
							}
							if (!stillLowest)
							{
								usedProjects.Remove(lowest);
								usedProjects.Add(pd);
							}
						}
					}
				}

				FinalGradeDTO returnValue = new FinalGradeDTO();

				//Calculate the grade
				foreach(ProjectData pd in usedProjects){
					returnValue.PercentageComplete += pd.Weight;
					returnValue.Grade += (int)pd.Grade / 1000.0 * pd.Weight;
				}

				//returnValue.Grade *= returnValue.PercentageComplete;
				return returnValue;
			}
		}

		#endregion Private helper classes

		#region Language methods
        public LanguageViewModel GetLanguageByName(string name)
        {

            //Throw exception on porpuse
            throw new CoursesAPIObjectNotFoundException(ErrorCodes.LanguageDoesNotExist);
        }

        public LanguageViewModel GetLanguageById(int id)
        {
            return new LanguageViewModel
            {
                Description = "Description",
                Timestamp = DateTime.UtcNow,
                Name = "Name"
            };            
        }


        //Dummy function representing the method for creating a instance of Language
        public LanguageViewModel CreateLanguage(LanguageViewModel model)
        {
            //Validate here!
            CourseAPIValidation.Validate(model);

            //TODO create the corrsponding instance in DB

            return model;
        }
		#endregion Language methods

		#region Course methods
		public List<Person> GetCourseTeachers(int courseInstanceID)
		{
			// TODO:
            var result = from tr in _teacherRegistrations.All()
                         join p in _persons.All() on tr.SSN equals p.SSN
                         where tr.CourseInstanceID == courseInstanceID
                         select p;

            var result2 = result.ToList();
            return result2;
		}

		public List<CourseInstanceDTO> GetCourseInstancesOnSemester(string semester)
		{
			// TODO:
            if (String.IsNullOrEmpty(semester))
            {
                semester = _semesters.All().OrderByDescending(x => x.DateBegins).Select(s => s.ID).FirstOrDefault();
            }

            var result = from ci in _courseInstances.All()
                         join c in _courseTemplates.All() on ci.CourseID equals c.CourseID
                         where ci.SemesterID == semester
                         select new CourseInstanceDTO
                         {
                             CourseInstanceID = ci.ID,
                             CourseID = ci.CourseID,
                             Name = c.Name,
                             MainTeacher = "Main teacher Name"
                         };

            return result.OrderBy(c => c.Name).ToList();
		}

		public List<CourseInstanceDTO> GetSemesterCourses(string semester)
		{
			// TODO
            if (String.IsNullOrEmpty(semester))
            {
                semester = _semesters.All().OrderByDescending(x => x.DateBegins).Select(s => s.ID).FirstOrDefault();
            }

            var result = from ci in _courseInstances.All()
                         join c in _courseTemplates.All() on ci.CourseID equals c.CourseID
                         where ci.SemesterID == semester
                         select new CourseInstanceDTO
                         {
                             CourseInstanceID = ci.ID,
                             CourseID = ci.CourseID,
                             Name = c.Name,
                             MainTeacher = "Main teacher Name"
                         };

            return result.OrderBy(c => c.Name).ToList();
		}
		#endregion

		#region project and grading methods

        /// <summary> 
        /// TODO
        /// </summary>
        public void AddProjectToCourse(int id, AddProjectViewModel model)
        {
            var course = _courseInstances.All().SingleOrDefault(c => c.ID == id);

            if(course == null)
            {
                throw new ArgumentException("Invalid course instance id");
            }

            
            var projectGroup = _projectGroups.All().SingleOrDefault(g => g.ID == model.ProjectGroupId);

            if(projectGroup == null && model.ProjectGroupId != null)
            {
                throw new ArgumentException("Invalid project-group id");
            }

			//TODO: Suppor ProjectGroupId and OnlyHigherThanProjectId
            Project project = new Project
            {
                Name = model.Name,
                CourseInstanceId = id,
                ProjectGroupId = model.ProjectGroupId,
                OnlyHigherThanProjectId = null,
                Weight = model.Weight,
                MinGradeToPassCourse = model.MinGradeToPassCourse
            };

            _projects.Add(project);
            _uow.Save();
        }

        /// <summary> 
        /// TODO
        /// </summary>
        public void MakeProjectGroup(AddProjectGroupViewModel model)
        {
            if(model == null)
            {
                throw new MissingFieldException("The payload must contain \"Name\" and \"GradedProjectsCount\"");
            }

            if (model.Name == null)
            {
                throw new MissingFieldException("A \"Name\" field is required");
            }

            if (!(model.GradedProjectsCount.HasValue))
            {
                throw new MissingFieldException("A \"GradedProjectsCount\" field is required");
            }

            ProjectGroup group = new ProjectGroup
            {
                Name = model.Name,
                GradedProjectsCount = model.GradedProjectsCount.Value
            };

            _projectGroups.Add(group);
            _uow.Save();
        }

		/// <summary>
		/// Creates lines in the FinalGradeCompositions table to indicate which projects
		/// should be used to calculate the final grade
		/// Replaces the old lines
		/// </summary>
		/// <param name="courseInstanceId">The id of the course instance</param>
		/// <param name="model">A list of projects that should be used</param>
		public void MakeFinalGradeComp(int courseInstanceId, AddFinalGradeCompositionViewModel model)
		{
			if (model == null)
			{
				throw new MissingFieldException("The payload must contain a \"Projects\" value as a list of integers");
			}

			//See if the courseInstance exists
			CourseInstance theCourse = _courseInstances.All().SingleOrDefault(c => c.ID == courseInstanceId);

			if (theCourse == null)
			{
				throw new KeyNotFoundException("No course instance found with this ID");
			}

			//TODO: Add validation


			//Remove the old composition
			List<FinalGradeComposition> currentComps = _finalGradeComps.All().Where(f => f.CourseInstanceId == courseInstanceId).ToList();
			
			foreach(FinalGradeComposition comp in currentComps){
				_finalGradeComps.Delete(comp);
			}

			foreach(int projectId in model.Projects){
				Project currentProject = null;
				try
				{
					currentProject = _projects.All().SingleOrDefault(p => p.ID == projectId && p.CourseInstanceId == courseInstanceId);
				}
				catch(Exception e){
					throw new Exception("More than one project found with the given ID");
				}
				if(currentProject == null){
					throw new KeyNotFoundException("No project found with the given ID in the given course");
				}
				_finalGradeComps.Add(new FinalGradeComposition{
									CourseInstanceId = courseInstanceId,
									ProjectId = projectId});
			}

			_uow.Save();
		}

        /// <summary> 
        /// TODO
        /// </summary>
		public void AddGrade(int courseInstanceID, int projectID, AddGradeViewModel viewModel)
		{
			if(viewModel == null){
				throw new MissingFieldException("The payload must contain \"Grade\" and \"PersonSSN\"");
			}
			if(viewModel.Grade == null){
				throw new MissingFieldException("A \"Grade\" field is required");
			}
			if (viewModel.PersonSSN == null)
			{
				throw new MissingFieldException("A \"PersonSSN\" field is required");
			}

			if(viewModel.Grade < 0 || viewModel.Grade > 100){
				throw new ArgumentException("The grade must be an integer value  between 0 and 100");
			}
			
			//See if the courseInstance exists
			CourseInstance theCourse = _courseInstances.All().SingleOrDefault(c => c.ID == courseInstanceID);

			if (theCourse == null)
			{
				throw new KeyNotFoundException("No course instance found with this ID");
			}

			//See if the project exist
			Project theProject = _projects.All().SingleOrDefault(p => p.ID == projectID);

			if (theProject == null)
			{
				throw new KeyNotFoundException("No project found with this ID in the given course instance");
			}

			//See if the person is in the course
			PersonRegistration thePerson = _personRegistrations.All().SingleOrDefault(p => p.PersonSSN == viewModel.PersonSSN 
																						&& p.CourseInstanceId == courseInstanceID);

			if(thePerson == null){
				throw new KeyNotFoundException("The given person is not registered in the given course");
			}

			//See if the person already has a grade for the project

			Grade theGrade = null;
			try
			{
				theGrade = _grades.All().SingleOrDefault(g => g.ProjectId == projectID
																&& g.PersonSSN == viewModel.PersonSSN);
			}
			catch(Exception e){
				//There are more than one grade that fit the criteria
				if(_grades.All().Count() != 0){
					throw new Exception("The given person already has more than one grade for the given project");
				}
				//The collection is empty
			}

			if(theGrade == null){
				//Add a new grade
				_grades.Add(new Grade
				{
					ProjectId = projectID,
					GradeValue = viewModel.Grade,
					PersonSSN = viewModel.PersonSSN
				});
			}
			else{
				//TODO: Should the old one be overwritten, or should there be an error
			}
		}

        /// <summary> 
        /// TODO
        /// </summary>
		public List<Project> GetProjectsForCourse(int id)
        {
            return _projects.All().ToList();
        }

        /// <summary> 
        /// Get student grade in project and return his grade, position based on all students grades
        /// and how many students have been graded
        /// </summary>
        public GradeDTO GetProjectGrade(int courseInstanceId, int projectId, String ssn)
        {
			//See if the courseInstance exists
			CourseInstance theCourse = _courseInstances.All().SingleOrDefault(c => c.ID == courseInstanceId);

			if (theCourse == null)
			{
				throw new KeyNotFoundException("No course instance found with this ID");
			}

			List<Grade> allGrades = (from gr in _grades.All()
									join p in _projects.All() on gr.ProjectId equals p.ID
									where p.CourseInstanceId == courseInstanceId && p.ID == projectId 
									select gr).ToList();


			int? myGrade = (from gr in allGrades
								where gr.PersonSSN == ssn && gr.ProjectId == projectId
								select gr.GradeValue).SingleOrDefault();

			GradeDTO returnValue = new GradeDTO();

			returnValue.NumberOfStudents = allGrades.Count();
			returnValue.Grade = myGrade;

			if(myGrade == null){
				returnValue.PositionLower = null;
				returnValue.PositionUpper = null;
				return returnValue;
			}
			else {
				int greater = 0;
				int equal = 0;
				foreach(Grade g in allGrades){
					if(g.GradeValue != null){
						if(g.GradeValue > myGrade){
							greater++;
						}
						else if(g.GradeValue == myGrade){
							equal++;
						}
					}
				}

				returnValue.PositionUpper = 1 + greater;
				returnValue.PositionLower = greater + equal;
				
				return returnValue;
			}
        }

        /// <summary> 
        /// TODO
        /// </summary>
		private FinalGradeDTO GetFinalGrade(int courseInstanceID, String personSSN)
		{
			if(personSSN == null){
				throw new MissingFieldException("The field \"personSSN\" is required");
			}

			//See if the courseInstance exists
			CourseInstance theCourse = _courseInstances.All().SingleOrDefault(c => c.ID == courseInstanceID);

			if (theCourse == null)
			{
				throw new KeyNotFoundException("No course instance found with this ID");
			}

			//See if a FinalGradeComposition is registered for the course
			List<FinalGradeComposition> theGradeComps = _finalGradeComps.All().Where(f => f.CourseInstanceId == courseInstanceID).ToList();

			if(theGradeComps == null){
				throw new KeyNotFoundException("The composition of the final grade has not been registered for this course");
			}

			FinalGradeDTO returnValue = new FinalGradeDTO
			{
				Grade = 0,
				PercentageComplete = 0,
				Status = "OK"
			};
			
			int totalPercentage = 0;
			Dictionary<int, ProjectGroupData> projectGroups = new Dictionary<int,ProjectGroupData>();

			foreach(var comp in theGradeComps){
				//TODO: Add handling for ProjectGroups and OnlyIfHigherThan
				FinalGradeComposition currentComp = (FinalGradeComposition)comp;
				Project currentProject = null;
				try
				{
					currentProject = _projects.All().SingleOrDefault(p => p.ID == comp.ProjectId);
				}
				catch(Exception e){
					if(_projects.All().Count() != 0){
						throw new Exception("There are more than one projects with the given ID");
					}
					//The collection is empty
				}

				GradeDTO currentGrade = GetProjectGrade(courseInstanceID, currentComp.ProjectId, personSSN);
				if(currentProject.MinGradeToPassCourse != null && currentGrade.Grade != null){
					if(currentGrade.Grade < currentProject.MinGradeToPassCourse){
						returnValue.Status = "FAILED";
					}
				}
				//Part of a ProjectGroup
				if(currentProject.ProjectGroupId != null){
					int currentID = (int)currentProject.ProjectGroupId;

					ProjectGroup currentProjectGroup = null;
					//See if the projectgroup exists
					try{
						currentProjectGroup = _projectGroups.All().SingleOrDefault(p => p.ID == currentID);
					}
					catch(Exception e){
						if(_projectGroups.All().Count() != 0){
							throw new KeyNotFoundException("No project group found with the given ID(" + currentID + ")");
						}
						//The collection is just empty
					}

					if(!projectGroups.ContainsKey(currentID)){
						//There is no defined projectgroup with this id
						projectGroups.Add(currentID, new ProjectGroupData(currentID, currentProjectGroup.GradedProjectsCount));
					}
					
					projectGroups[currentID].AddProject(currentGrade.Grade, currentProject.Weight);
				}
				else
				{
					totalPercentage += currentProject.Weight;

					if(currentGrade != null){
						returnValue.PercentageComplete += currentProject.Weight;
						returnValue.Grade += (int)currentGrade.Grade / 1000.0 * currentProject.Weight;
					}
				}


				
			}

			FinalGradeDTO temp;
			foreach(ProjectGroupData pgd in projectGroups.Values){
				temp = pgd.getTotalGrade();
				returnValue.Grade += temp.Grade;
				returnValue.PercentageComplete += temp.PercentageComplete;
				totalPercentage += pgd.TheProjects[0].Weight * pgd.GradedProjectsCount;
			}

			if(totalPercentage != 100){
				throw new Exception("Total weight of the components of the final grade is " + totalPercentage + ", not 100 as it should be.");
			}

			returnValue.PersonSSN = personSSN;
			return returnValue;
		}

        /// <summary> 
        /// TODO
        /// </summary>
		public FinalGradeDTO GetFinalGradeForOneStudent(int courseInstanceID, String personSSN)
		{
			List<FinalGradeDTO> allFinalGrades = GetAllFinalGrades(courseInstanceID);


			return allFinalGrades.SingleOrDefault(f => f.PersonSSN == personSSN);
		}

        /// <summary> 
        /// TODO
        /// </summary>
		public List<FinalGradeDTO> GetAllFinalGrades(int courseInstanceId)
		{
			//See if the courseInstance exists
			CourseInstance theCourse = _courseInstances.All().SingleOrDefault(c => c.ID == courseInstanceId);

			if (theCourse == null)
			{
				throw new KeyNotFoundException("No course instance found with this ID");
			}

			//See if a FinalGradeComposition is registered for the course
			List<FinalGradeComposition> theGradeComps = _finalGradeComps.All().Where(f => f.CourseInstanceId == courseInstanceId).ToList();

			if (theGradeComps == null)
			{
				throw new KeyNotFoundException("The composition of the final grade has not been registered for this course");
			}

			//Get a list of all persons in the course
			List<String> personsRegistered = _personRegistrations.All().Where(r => r.CourseInstanceId == courseInstanceId).Select(f => f.PersonSSN).ToList();

			List<FinalGradeDTO> finalGrades = new List<FinalGradeDTO>();
			foreach(String reg in personsRegistered){
				finalGrades.Add(GetFinalGrade(courseInstanceId, reg));
			}

			int count = finalGrades.Count();
			
			

			foreach(FinalGradeDTO fdto in finalGrades){
				fdto.NumberOfStudents = count;
				if(fdto.Grade == null){
					fdto.PositionUpper = null;
					fdto.PositionLower = null;
				}
				else {
					int equals = 0;
					int greater = 0;
					foreach(FinalGradeDTO f in finalGrades){
						if(f.Grade > fdto.Grade){
							greater++;
						}
						else if(f.Grade == fdto.Grade){
							equals++;
						}
					}

					fdto.PositionUpper = 1 + greater;
					fdto.PositionLower = greater + equals;
				}
			}

			return finalGrades;
		}

        /// <summary> 
        /// Get all students grades in projectID and return as List<PersonsGrade>
        /// </summary>
        // TODO: just a simple return with all grades without any other info
        public List<PersonsGrade> GetAllGrades(int projectId)
        {
            if(projectId == null)
            {
                throw new MissingFieldException("The id of the project is missing");
            }

            var result = (from gr in _grades.All()
                          join ps in _persons.All() on gr.PersonSSN equals ps.SSN
                          where gr.ProjectId == projectId &&
                          gr.GradeValue != null
                          select new PersonsGrade
                          {
                              PersonSSN = ps.SSN,
                              Name = ps.Name, 
                              Grade = (gr.GradeValue != null ? (double)gr.GradeValue/10 : 0)
                          }).ToList();

            if(result == null)
            {
                throw new KeyNotFoundException("No grades have been made");
            }

            return result;
        }

		#endregion
	}
}

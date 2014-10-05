using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Models.Entities;
using CoursesAPI.Services.Helpers;
using CoursesAPI.Services.Exceptions;
using CoursesAPI.Services.Extensions;

namespace CoursesAPI.Services.Services
{
	public class CoursesServiceProvider
	{
		#region Database collections
		public readonly IUnitOfWork _uow;

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

        /// <summary> 
        /// Contructor for CoursesServiceProvider. 
        /// Argument is IUnitOfWork
        /// </summary>
        /// <param name="uow">Unit of work</param>
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
        /// <summary>
        /// A helper class that contains information about projects in a project group
		/// Used in order to calculate the x best grades out of y projects
        /// </summary>
		private class ProjectGroupData{
			public int ProjectGroupID { get; set; }
			public int GradedProjectsCount { get; set; }
			public List<ProjectData> TheProjects = new List<ProjectData>();

            /// <summary>
            /// Creates an instance of the class
            /// </summary>
            /// <param name="ID">The ID of the project group</param>
            /// <param name="GradedProjectsCount">The number of projects that are used
			/// to calculate the grade for the project group</param>
			public ProjectGroupData(int ID, int GradedProjectsCount)
			{
				this.ProjectGroupID = ID;
				this.GradedProjectsCount = GradedProjectsCount;
			}

            /// <summary>
            /// Adds a project to the project group to be used for calculations
            /// </summary>
            /// <param name="Grade">The student's grade for the project</param>
            /// <param name="Weight">The weight of the project</param>
			public void AddProject(int? Grade, int Weight)
			{
				//This causes a NullReferenceException if Grade == null
				this.TheProjects.Add(new ProjectData { 
										Grade = Grade,
										Weight = Weight
									});
			}

            /// <summary>
            /// A helper class for ProjectGroupData, to contain information
			/// about the projects in the project group
            /// </summary>
			public class ProjectData
			{
				public int? Grade { get; set; }
				public int Weight { get; set; }
			}

            /// <summary>
            /// Checks if the weights of all the projects in the project group are the same
            /// </summary>
            /// <returns></returns>
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

            /// <summary>
            /// Calculates the total finalgrade value of the project group
            /// </summary>
            /// <returns>The grade for the project group
			/// and the combined weight of the project group</returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LanguageViewModel GetLanguageByName(string name)
        {

            //Throw exception on porpuse
            throw new CoursesAPIObjectNotFoundException(ErrorCodes.LanguageDoesNotExist);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LanguageViewModel GetLanguageById(int id)
        {
            return new LanguageViewModel
            {
                Description = "Description",
                Timestamp = DateTime.UtcNow,
                Name = "Name"
            };            
        }

        /// <summary>
        /// Dummy function representing the method for creating a instance of Language
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public LanguageViewModel CreateLanguage(LanguageViewModel model)
        {
            //Validate here!
            CourseAPIValidation.Validate(model);

            //TODO create the corrsponding instance in DB

            return model;
        }
		#endregion Language methods

		#region Course methods

        /// <summary>
        /// Gets all teachers in a given course
        /// </summary>
        /// <param name="courseInstanceID">Id of the course</param>
        /// <returns>All teachers in given course</returns>
		public List<Person> GetCourseTeachers(int courseInstanceID)
		{
			// TODO:
            if(courseInstanceID == null)
            {
                throw new MissingFieldException("Must provide id of the course");
            }

            CourseInstance theCourse = _courseInstances.All().SingleOrDefault(c => c.ID == courseInstanceID);

            if(theCourse == null)
            {
                throw new KeyNotFoundException("There is no course with this id");
            }

            var result = from tr in _teacherRegistrations.All()
                         join p in _persons.All() on tr.SSN equals p.SSN
                         where tr.CourseInstanceID == theCourse.ID
                         select p;

            var result2 = result.ToList();
            return result2;
		}

        /// <summary>
        /// Gets all courses on given semester
        /// </summary>
        /// <param name="semester">Semester name</param>
        /// <returns>All courses on given semester</returns>
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

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="semester"></param>
        /// <returns></returns>
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
        /// Adds project from a viewmodel to a given course
        /// </summary>
        /// <param name="id">Id of the course</param>
        /// <param name="model">Viewmodel containing objects to put in the current project</param>
        public void AddProjectToCourse(int id, AddProjectViewModel model)
        {
            if ((PercentCompleted(id, model)) > 100)
            {
                throw new ApplicationException("Cannot add project, total precent becomes: " +
                                     (PercentCompleted(id, model)) + ", it cant be higher than 100");
            }

            var course = _courseInstances.GetCourseByID(id);
            
            var projectGroup = _projectGroups.All().SingleOrDefault(g => g.ID == model.ProjectGroupId);

            if(projectGroup == null && model.ProjectGroupId != null)
            {
                throw new KeyNotFoundException("No group found with this ID");
            }

            Project project = new Project
            {
                Name = model.Name,
                CourseInstanceId = id,
                ProjectGroupId = model.ProjectGroupId,
                OnlyHigherThanProjectId = model.OnlyHigherThanProjectId,
                Weight = model.Weight,
                MinGradeToPassCourse = model.MinGradeToPassCourse
            };

            _projects.Add(project);
            _uow.Save();
        }

        /// <summary>
        /// Removes project from a given course
        /// </summary>
        /// <param name="courseId">Id of the course</param>
        /// <param name="projectId">Id of the project</param>
        public void RemoveProjectFromCourse(int courseId, int projectId)
        {
            var course = _courseInstances.GetCourseByID(courseId);

            try
            {
                var project = _projects.All().SingleOrDefault(x => x.ID == projectId);

                if (project == null)
                {
                    throw new Exception("No project found with that id");
                }

                _projects.Delete(project);
                _uow.Save();
            }
            catch (Exception)
            {
                throw new Exception("Found two projects with same id");
            }
        }

        /// <summary>
        /// Ceck if a user can add a project without going over 100 percent
        /// </summary>
        /// <param name="id">Id of the course</param>
        /// <param name="model">viewmodel of project to add</param>
        public int PercentCompleted(int id, AddProjectViewModel model)
        {
            CourseInstance theCourse = _courseInstances.GetCourseByID(id);

            int PrecentComplete = 0;

            Dictionary<int, int> myLists = new Dictionary<int, int>();

            var projects = GetProjectsForCourse(id);

            foreach (Project p in projects)
            {
                if (p.ProjectGroupId == null)
                {
                    PrecentComplete += p.Weight;
                }
                else
                {
                    if (!(myLists.ContainsKey(p.ProjectGroupId.Value)))
                    {
                        myLists.Add(p.ProjectGroupId.Value, p.Weight);
                    }
                }
            }

            bool groupexist = false;
            foreach (KeyValuePair<int, int> pair in myLists)
            {
                var group = _projectGroups.All().SingleOrDefault(x => x.ID == pair.Key);

                if (group == null)
                {
                    throw new Exception("Found two or more project groups with the same id");
                }
                if (model.ProjectGroupId == pair.Key)
                {
                    groupexist = true;
                }
                PrecentComplete += group.GradedProjectsCount * pair.Value;
            }

            if (!groupexist && model.ProjectGroupId != null)
            {
                var group = _projectGroups.All().SingleOrDefault(x => x.ID == model.ProjectGroupId);

                PrecentComplete += model.Weight * group.GradedProjectsCount;
            }

            if(model.ProjectGroupId == null)
            {
                PrecentComplete += model.Weight;
            }

            return PrecentComplete;
        }

        /// <summary>
        /// Creates a group for projects so the teacher could allow some number of projects to be 
        /// evaluated for final grade, e.g. grades from top three projects and then the final grade is
        /// the sum of the grades divided by their count.
        /// </summary>
        /// <param name="model">A name of the group and number of projects to be evaluated</param>
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
        /// <param name="courseInstanceId">Id of the course</param>
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

			
			//Validate the viewmodel
			List<Project> theProjects = _projects.All().Where(p => model.Projects.Contains(p.ID)).ToList();
			int totalWeight = 0;
			Dictionary<int, ProjectGroupData> theProjectGroups = new Dictionary<int,ProjectGroupData>();

			foreach(Project p in theProjects){
				if(p.ProjectGroupId == null){
					totalWeight += p.Weight;
				}
				else
				{
					ProjectGroup pGroup = null;
					try {
						pGroup = _projectGroups.All().SingleOrDefault(g => g.ID == p.ProjectGroupId);
					}
					catch(Exception e){
						if(_projectGroups.All().Count() != 0){
							throw new Exception("More than one project groups found with the given ID");
						}
						else
						{
							//The collection is empty
							throw new KeyNotFoundException("There is no project group with the given ID");
						}

					}
					if(!theProjectGroups.ContainsKey((int)p.ProjectGroupId)){
						theProjectGroups.Add((int)p.ProjectGroupId, new ProjectGroupData(pGroup.ID, pGroup.GradedProjectsCount));
					}
					theProjectGroups[(int)p.ProjectGroupId].AddProject(0, p.Weight);
				}
			}

			foreach(ProjectGroupData p in theProjectGroups.Values){
				totalWeight += Math.Min(p.GradedProjectsCount, p.TheProjects.Count()) * p.TheProjects[0].Weight;
			}

			if(totalWeight != 100){
				throw new ArgumentException("The total weight of the projects should be 100, not " + totalWeight);
			}

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
        /// Adds grade to a project based on information from the AddGradeViewModel containing
        /// student id and his grade.
        /// </summary>
        /// <param name="courseInstanceID">Id of the course</param>
        /// <param name="projectID">Id of the project</param>
        /// <param name="viewModel">Contains student id to be evaluated and his grade</param>
		public void SetGrade(int courseInstanceID, int projectID, AddGradeViewModel viewModel)
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
				//Overwrite the old grade
				theGrade.GradeValue = viewModel.Grade;
			}
			_uow.Save();
		}

        /// <summary>
        /// Gets all project with the given id as courseId
        /// </summary>
        /// <param name="id">the course Id</param>
        /// <returns>A list of projects</returns>
        public List<Project> GetProjectsForCourse(int id)
        {

            var result = from pro in _projects.All()
                         where pro.CourseInstanceId == id
                         select pro;

            var resultlist = result.ToList();

            return resultlist;
        }

        /// <summary>
        /// Get student grade in project and return his grade, position based on all students grades
        /// and how many students have been graded
        /// </summary>
        /// <param name="courseInstanceId">Id of the course</param>
        /// <param name="projectId">The ID of the project</param>
        /// <param name="ssn">The SSN of the student</param>
        /// <returns>List with grade, position and how many students are evaluated</returns>
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
            returnValue.SSN = ssn;

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
        /// Gets final grade of the course based on student id.
        /// </summary>
        /// <param name="courseInstanceID">Id of the course</param>
        /// <param name="personSSN">Id of the student</param>
        /// <returns>A list of student final grade with his status, as in \"Failed\" or \"OK\",
        /// percentage of completed projects, his position in the course and number of students</returns>
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
			List<int> finishedIDs = new List<int>();

			foreach(var comp in theGradeComps){
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


				if (!finishedIDs.Contains(currentProject.ID))
				{
					//Part of a ProjectGroup
					if (currentProject.ProjectGroupId != null)
					{
						int currentID = (int)currentProject.ProjectGroupId;

						ProjectGroup currentProjectGroup = null;
						//See if the projectgroup exists
						try
						{
							currentProjectGroup = _projectGroups.All().SingleOrDefault(p => p.ID == currentID);
						}
						catch (Exception e)
						{
							if (_projectGroups.All().Count() != 0)
							{
								throw new KeyNotFoundException("No project group found with the given ID(" + currentID + ")");
							}
							//The collection is just empty
						}

						if (!projectGroups.ContainsKey(currentID))
						{
							//There is no defined projectgroup with this id
							projectGroups.Add(currentID, new ProjectGroupData(currentID, currentProjectGroup.GradedProjectsCount));
						}

						projectGroups[currentID].AddProject(currentGrade.Grade, currentProject.Weight);
					}
					else
					{
						totalPercentage += currentProject.Weight;

						if (currentGrade != null)
						{
							if (currentGrade.Grade != null)
							{
								if (currentProject.OnlyHigherThanProjectId == null)
								{
									returnValue.Grade += (int)currentGrade.Grade / 1000.0 * currentProject.Weight;
									returnValue.PercentageComplete += currentProject.Weight;
									finishedIDs.Add(currentProject.ID);
								}
								else
								{
									//Grade otherGrade = _grades.All().SingleOrDefault(g => g.ProjectId == currentProject.OnlyHigherThanProjectId && g.PersonSSN == personSSN);
									GradeDTO otherGrade = GetProjectGrade(courseInstanceID, (int)currentProject.OnlyHigherThanProjectId, personSSN);
									if (otherGrade.Grade > currentGrade.Grade)
									{
										Project otherProject = _projects.All().SingleOrDefault(p => p.ID == currentProject.OnlyHigherThanProjectId);
										

										//Increase the weight of the other project
										if (finishedIDs.Contains((int)currentProject.OnlyHigherThanProjectId))
										{
											//We have already added the other project
											returnValue.Grade -= (int)otherGrade.Grade / 1000.0 * otherProject.Weight;
											returnValue.PercentageComplete -= otherProject.Weight;
										}

										returnValue.Grade += (int)otherGrade.Grade / 1000.0 * (otherProject.Weight + currentProject.Weight);
										returnValue.PercentageComplete += currentProject.Weight + otherProject.Weight;
										finishedIDs.Add(currentProject.ID);
									}
									else
									{
										returnValue.Grade += (int)currentGrade.Grade / 1000.0 * currentProject.Weight;
										returnValue.PercentageComplete += currentProject.Weight;
										finishedIDs.Add(currentProject.ID);
									}
								}
							}
						}
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

			returnValue.Grade *= 2;
			returnValue.Grade = Math.Round(returnValue.Grade, MidpointRounding.AwayFromZero);
			returnValue.Grade /= 2;

			return returnValue;
		}

        /// <summary>
        /// Gets the final grade from the course of given student.
        /// </summary>
        /// <param name="courseInstanceID">Id of the course</param>
        /// <param name="personSSN">Id of the student</param>
        /// <returns>A list of student final grade with his status, as in \"Failed\" or \"OK\",
        /// percentage of completed projects, his position in the course and number of students</returns>
		public FinalGradeDTO GetFinalGradeForOneStudent(int courseInstanceID, String personSSN)
		{

			PersonRegistration reg = null;
			try{
				reg = _personRegistrations.All().Single(r => r.CourseInstanceId == courseInstanceID
																		&& r.PersonSSN == personSSN);
			}
			catch(Exception e){
				if(_personRegistrations.All().Count() != 0){
					throw new Exception("The given student is registered more than once into the given course");
				}
				//The collection is empty
			}

			if(reg == null){
				throw new KeyNotFoundException("The student is not registered in the given course");
			}

			List<FinalGradeDTO> allFinalGrades = GetAllFinalGrades(courseInstanceID);
			
			return allFinalGrades.SingleOrDefault(f => f.PersonSSN == personSSN);
		}

        /// <summary>
        /// Gets final grades of all student in given course
        /// </summary>
        /// <param name="courseInstanceId">Id of the course</param>
        /// <returns>A list of students final grades with their status, as in \"Failed\" or \"OK\",
        /// percentage of completed projects, their position in the course and number of students</returns>
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
        /// <param name="projectId">A list of projects that should be used</param>
        /// <returns></returns>
        public List<GradeDTO> GetAllGrades(int courseInstanceId, int projectId)
        {
            //See if the courseInstance exists
            CourseInstance theCourse = _courseInstances.All().SingleOrDefault(c => c.ID == courseInstanceId);

            if (theCourse == null)
            {
                throw new KeyNotFoundException("No course instance found with this ID");
            }

            // See if projects does exist
            List<Project> theProjects = (from p in _projects.All()
                                         join c in _courseInstances.All() on p.CourseInstanceId equals c.ID
                                         where p.ID == projectId && c.ID == courseInstanceId
                                         select p).ToList();

            if (theProjects.Count == 0)
            {
                throw new KeyNotFoundException("No project instance found with this ID");
            }

            //Get a list of all persons in the course
            List<String> studentsInProject = (from gr in _grades.All()
                                              where gr.ProjectId == projectId
                                              select gr.PersonSSN).ToList();
                         
            
            // Create a new list containing all the grades from the project
            List<GradeDTO> result = new List<GradeDTO>();

            // Add each student to a list
            foreach (String reg in studentsInProject)
            {
                result.Add(GetProjectGrade(courseInstanceId, projectId, reg));
            }

            return result;
        }

		#endregion
	}
}

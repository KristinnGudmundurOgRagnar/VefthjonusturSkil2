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
		#endregion Database collections

		public CoursesServiceProvider(IUnitOfWork uow)
		{
			_uow = uow;

			_courseInstances      = _uow.GetRepository<CourseInstance>();
			_courseTemplates      = _uow.GetRepository<CourseTemplate>();
			_teacherRegistrations = _uow.GetRepository<TeacherRegistration>();
			_persons              = _uow.GetRepository<Person>();

			_personRegistrations  = _uow.GetRepository<PersonRegistration>();
			_projectGroups		  = _uow.GetRepository<ProjectGroup>();
			_projects             = _uow.GetRepository<Project>();
			_grades               = _uow.GetRepository<Grade>();
		}

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

        public void AddProjectToCourse(int id, AddProjectViewModel model)
        {
            var course = _courseInstances.All().SingleOrDefault(c => c.ID == id);

            if(course == null)
            {
                throw new ArgumentException("Invalid course instance id");
            }

            Project project = new Project
            {
                Name = model.Name,
                ProjectGroupId = null,
                CourseInstanceId = id,
                OnlyHigherThanProjectId = null,
                Weight = model.Weight,
                MinGradeToPassCourse = model.MinGradeToPassCourse
            };

            _projects.Add(project);
            _uow.Save();
        }

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
			Grade theGrade = _grades.All().SingleOrDefault(g => g.ProjectId == projectID
															&& g.PersonSSN == viewModel.PersonSSN);

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

        public List<Project> GetProjectsForCourse(int id)
        {
            return _projects.All().ToList();
        }

        public int? getProjectGrade(int courseInstanceId, int projectId, String ssn)
        {
            var result = (from gr in _grades.All()
                          where gr.ProjectId == projectId &&
                          gr.PersonSSN == ssn
                          select gr.GradeValue).FirstOrDefault();
            return result;
        }

		#endregion
	}
}

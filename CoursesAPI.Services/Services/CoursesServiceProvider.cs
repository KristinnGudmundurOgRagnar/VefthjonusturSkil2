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
		private readonly IUnitOfWork _uow;

		private readonly IRepository<CourseInstance> _courseInstances;
		private readonly IRepository<TeacherRegistration> _teacherRegistrations;
		private readonly IRepository<CourseTemplate> _courseTemplates; 
		private readonly IRepository<Person> _persons;
        private readonly IRepository<Semester> _semesters;

		private readonly IRepository<ProjectGroup> _projectGroups;
		private readonly IRepository<Project> _projects;
		private readonly IRepository<Grade> _grades;

		public CoursesServiceProvider(IUnitOfWork uow)
		{
			_uow = uow;

			_courseInstances      = _uow.GetRepository<CourseInstance>();
			_courseTemplates      = _uow.GetRepository<CourseTemplate>();
			_teacherRegistrations = _uow.GetRepository<TeacherRegistration>();
			_persons              = _uow.GetRepository<Person>();
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

        public List<Project> GetProjectsForCourse(int id)
        {
            return _projects.All().ToList();
        }
		#endregion
	}
}

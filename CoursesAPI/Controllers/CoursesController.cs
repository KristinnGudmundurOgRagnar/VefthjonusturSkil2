using System.Collections.Generic;
using System.Web.Http;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Models.Entities;
using CoursesAPI.Services.Services;
using System;

namespace CoursesAPI.Controllers
{
    /// <summary>
    /// A controller for "course" part of the project
    /// </summary>
	[RoutePrefix("api/courses")]
	public class CoursesController : ApiController
	{
        /// <summary>
        /// Service provider instance
        /// </summary>
		private readonly CoursesServiceProvider _service;

        /// <summary>
        /// A constructor
        /// </summary>
		public CoursesController()
		{
			_service = new CoursesServiceProvider(new UnitOfWork<AppDataContext>());
		}

        /// <summary>
        /// Get the teachers from a course
        /// </summary>
        /// <param name="courseInstanceID">Id of the course</param>
        /// <returns>List of teachers of the course</returns>
		[Route("{courseInstanceID}/teachers")]
		public List<Person> GetCourseTeachers(int courseInstanceID)
		{

			return _service.GetCourseTeachers(courseInstanceID);
		}
		
        /// <summary>
        /// Get courses on selected semester
        /// </summary>
        /// <param name="semester">The id of the semester e.g. 20143</param>
        /// <returns>List of courses in selected semester</returns>
		[Route("semester/{semester}")]
		public List<CourseInstanceDTO> GetCoursesOnSemester(string semester)
		{
			return _service.GetSemesterCourses(semester);
		}

        /// <summary>
        /// Get courses on selected semester
        /// </summary>
        /// <param name="semester">The id of the semester e.g. 20143</param>
        /// <returns>List of courses in selected semester</returns>
        [Route("semester2/{semester}")]
        public List<CourseInstanceDTO> GetCoursesOnSemester2(string semester)
        {
            return _service.GetCourseInstancesOnSemester(semester);
        }
	}
}

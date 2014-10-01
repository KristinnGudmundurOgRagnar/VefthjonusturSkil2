using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoursesAPI.Services.Services;
using CoursesAPI.Models;
using CoursesAPI.Services.Models;
using CoursesAPI.Services.DataAccess;

namespace CoursesAPI.Controllers
{
	[RoutePrefix("api/courses/{courseInstanceID}")]
    public class ProjectController : ApiController
    {
		private readonly CoursesServiceProvider _service;

		public ProjectController()
		{
			_service = new CoursesServiceProvider(new UnitOfWork<AppDataContext>());
		}

		//TODO: Add ProjectViewModel and correct return type
		[HttpPost]
		[Route("project")]
        public HttpResponseMessage AddProject(int courseInstanceID, AddProjectViewModel model)
		{
            _service.AddProjectToCourse(courseInstanceID, model);
            return Request.CreateResponse(System.Net.HttpStatusCode.Created);
		}

		//TODO: Add GradeViewModel and correct return type
		[HttpPut]
		[Route("project/grade")]
		public List<int> AddGrade(int courseInstanceID)
		{

			return new List<int> { 1, 2, 3, 4, 5, 6, 7 };
		}

		//TODO: Add correct return type
		[HttpGet]
		[Route("project/{projectID}/grade")]
		public int GetGrade(int courseInstanceId, int projectId, int personId)
		{
			return 0;
		}


		//TODO: Add correct return type
		[HttpGet]
		[Route("finalGrade")]
		public int GetFinalGrade(int courseInstanceId, int personId)
		{
			return 0;
		}
    }
}

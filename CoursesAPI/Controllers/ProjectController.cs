using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoursesAPI.Services.Services;
using CoursesAPI.Models;
using CoursesAPI.Services.Models.Entities;
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

        [HttpGet]
        [Route("project")]
        public List<Project> GetProjects(int courseInstanceID)
        {
            return _service.GetProjectsForCourse(courseInstanceID);
        }

		[HttpPost]
		[Route("project")]
        public HttpResponseMessage AddProject(int courseInstanceID, AddProjectViewModel model)
		{
            _service.AddProjectToCourse(courseInstanceID, model);
            return Request.CreateResponse(System.Net.HttpStatusCode.Created);
		}

		
		[HttpPost]
		[Route("project/{projectId}/grade")]
		public void AddGrade(int courseInstanceID, int projectID, AddGradeViewModel viewModel)
		{
			try
			{
				_service.AddGrade(courseInstanceID, projectID, viewModel);
			}
			//TODO: Handle different exceptions differently
			catch (ArgumentException e)
			{
				HttpError theError = new HttpError();
				theError.Add("Error message", e.Message);
				HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, theError);
				throw new HttpResponseException(response);
			}
			catch(KeyNotFoundException e){
				HttpError theError = new HttpError();
				theError.Add("Error message", e.Message);
				HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound, theError);
				throw new HttpResponseException(response);
			}
			catch(Exception e){
				HttpError theError = new HttpError();
				theError.Add("Error message", e.Message);
				HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, theError);
				throw new HttpResponseException(response);
			}
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

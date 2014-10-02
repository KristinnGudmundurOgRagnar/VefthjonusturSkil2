﻿using System;
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
	[RoutePrefix("api/courses/{courseInstanceID:int}")]
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
		[Route("project/{projectId:int}/grade")]
		public HttpResponseMessage AddGrade(int courseInstanceID, int projectID, AddGradeViewModel viewModel)
		{
			try
			{
				_service.AddGrade(courseInstanceID, projectID, viewModel);
			}
			catch (ArgumentException e)
			{
				return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, e.Message);
			}
			catch(KeyNotFoundException e){
				return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, e.Message);
			}
			catch(Exception e){
				return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, e.Message);
			}

			return Request.CreateResponse(System.Net.HttpStatusCode.Created, "Grade created");
		}

        //TODO: Need to decide about the SSN in route
        // public List<int> GetGrade(int courseInstanceId, int projectId, ProjectViewModel personId)
        [HttpGet]
        [Route("project/{projectID:int}/grade/{ssn}")]
        public HttpResponseMessage GetGrade(int courseInstanceId, int projectId, String ssn)
        {
            HttpResponseMessage response;
            // Result could give us null so we need to take care of that
            var result = _service.getProjectGrade(courseInstanceId, projectId, ssn);

            if (result == null)
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            else
                response = Request.CreateResponse(HttpStatusCode.OK, result);

            return response;
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

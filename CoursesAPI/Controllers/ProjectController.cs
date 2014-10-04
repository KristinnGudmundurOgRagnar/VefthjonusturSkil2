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
	[RoutePrefix("api/courses/{courseInstanceID:int}")]
    public class ProjectController : ApiController
    {
		private readonly CoursesServiceProvider _service;

		public ProjectController()
		{
			_service = new CoursesServiceProvider(new UnitOfWork<AppDataContext>());
		}

		/// <summary>
		/// Get a list of all the projects in a given course
		/// </summary>
		/// <param name="courseInstanceID">The Id of the courseInstance, gotten from the URL</param>
		/// <returns>A list of all the projects in the course</returns>
        [HttpGet]
        [Route("project")]
        public List<Project> GetProjects(int courseInstanceID)
        {
            return _service.GetProjectsForCourse(courseInstanceID);
        }

		/// <summary>
		/// Add a new project to the course
		/// </summary>
		/// <param name="courseInstanceID">The Id of the courseInstance, gotten from the URL</param>
		/// <param name="model">The project that is to be added, gotten from the request payload</param>
		/// <returns>Status code, depending on the correctness of the payload</returns>
        [HttpPost]
        [Route("project")]
        public HttpResponseMessage AddProject(int courseInstanceID, AddProjectViewModel model)
        {

            if ((_service.PrecentCompleted(courseInstanceID, model)) > 100)
            {
                throw new Exception("Cannot add project, total precent becomes: " +
                                     (_service.PrecentCompleted(courseInstanceID, model)) + ", it cant be higher than 100");
            }

            _service.AddProjectToCourse(courseInstanceID, model);

            return Request.CreateResponse(System.Net.HttpStatusCode.Created);
        }

        [HttpDelete]
        [Route("project/{projectId:int}")]
        public void DeleteProject(int courseInstanceId, int projectId)
        {
            _service.RemoveProjectFromCourse(courseInstanceId, projectId);
        }

        [HttpPost]
        [Route("projectgroup")]
        public void MakeProjectGroup(int courseInstanceID, AddProjectGroupViewModel model)
        {
            _service.MakeProjectGroup(model);
        }


		[HttpPut]
		[Route("finalGradeComposition")]
		public HttpResponseMessage MakeFinalGradeComposition(int courseInstanceId, AddFinalGradeCompositionViewModel model)
		{
			try
			{
				_service.MakeFinalGradeComp(courseInstanceId, model);
			}
			catch (ArgumentException e)
			{
				return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, e.Message);
			}
			catch (MissingFieldException e)
			{
				return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, e.Message);
			}
			catch (KeyNotFoundException e)
			{
				return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, e.Message);
			}
			catch (Exception e)
			{
				return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, e.Message);
			}

			return Request.CreateResponse(System.Net.HttpStatusCode.Created, "Final grade composition created");
		}
		/// <summary>
		/// Add a grade for a given person for a given project
		/// </summary>
		/// <param name="courseInstanceID">The Id of the courseInstance, gotten from the URL</param>
		/// <param name="projectID">The Id of the project, gotten from the URL</param>
		/// <param name="viewModel">The grade that is to be added, gotten from the payload</param>
		/// <returns>Status code, depending on the correctness of the payload</returns>
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
			catch (MissingFieldException e)
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
        public HttpResponseMessage GetProjectGrade(int courseInstanceId, int projectId, String ssn)
        {
            GradeDTO result;

            try
            {
                result = _service.GetProjectGrade(courseInstanceId, projectId, ssn);
            }
            catch (MissingFieldException e)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, e.Message); 
            }
            catch (KeyNotFoundException e)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, result);
        }

		//TODO: Add correct return type
		[HttpGet]
		[Route("finalGrade/{personSSN}")]
		public HttpResponseMessage GetFinalGrade(int courseInstanceId, String personSSN)
		{
			FinalGradeDTO result;
			try
			{
				result = _service.GetFinalGradeForOneStudent(courseInstanceId, personSSN);
			}
			catch (ArgumentException e)
			{
				return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, e.Message);
			}
			catch (MissingFieldException e)
			{
				return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, e.Message);
			}
			catch (KeyNotFoundException e)
			{
				return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, e.Message);
			}
			catch (Exception e)
			{
				return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, e.Message);
			}


			return Request.CreateResponse(HttpStatusCode.OK, result);
		}

		[HttpGet]
		[Route("finalGrade/all")]
		public HttpResponseMessage GetAllFinalGrades(int courseInstanceId)
		{
			//TODO: Implement
			List<FinalGradeDTO> result = _service.GetAllFinalGrades(courseInstanceId);

			return Request.CreateResponse(HttpStatusCode.OK, result);
		}

        [HttpGet]
        [Route("project/{projectId:int}/allGrades")]
        public HttpResponseMessage GetAllGrades(int courseInstanceId, int projectId)
        {
            List<GradeDTO> result;
            try
            {
                result = _service.GetAllGrades(courseInstanceId, projectId);
            }
            catch (MissingFieldException e)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}

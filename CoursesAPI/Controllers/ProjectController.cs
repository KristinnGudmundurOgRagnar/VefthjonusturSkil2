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
    /// <summary>
    /// Controller API for all courses
    /// </summary>
	[RoutePrefix("api/courses/{courseInstanceID:int}")]
    public class ProjectController : ApiController
    {
        /// <summary>
        /// Service provider instance
        /// </summary>
		private readonly CoursesServiceProvider _service;

        /// <summary>
        /// Constructor fecthes our CoursesServiceProvider
        /// </summary>
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
        public HttpResponseMessage GetProjects(int courseInstanceID)
        {
            List<Project> projects;

            try
            {
                 projects = _service.GetProjectsForCourse(courseInstanceID);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, projects);
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
            try
            {
                _service.AddProjectToCourse(courseInstanceID, model);
            }
            catch (KeyNotFoundException e)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (ApplicationException e)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, e.Message);
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.Created);
        }

        /// <summary>
        /// Deletes a project from the course
        /// </summary>
        /// <param name="courseInstanceId">The Id of the courseInstance, gotten from the URL</param>
        /// <param name="model">The project that is to be deleted, gotten from the request payload</param>
        [HttpDelete]
        [Route("project")]
        public HttpResponseMessage DeleteProject(int courseInstanceId, DeleteProjectViewModel model)
        {
            try
            {
                _service.RemoveProjectFromCourse(courseInstanceId, model);
            }
            catch (KeyNotFoundException e)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Project has been deleted");
        }

        /// <summary>
        /// Makes a new project-group
        /// </summary>
        /// <param name="courseInstanceID">The Id of the courseInstance, gotten from the URL</param>
        /// <param name="model">The project-group that is to be added, gotten from the request payload</param>
        /// <returns>Status code, depending on the correctness of the payload</returns>
        [HttpPost]
        [Route("projectgroup")]
        public HttpResponseMessage MakeProjectGroup(int courseInstanceID, AddProjectGroupViewModel model)
        {
            try
            {
                _service.MakeProjectGroup(courseInstanceID, model);
            }
            catch (MissingFieldException e)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, e.Message);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, e.Message);
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.Created, "Project-group created");
        }

        /// <summary>
        /// Creates a composition of all final grades based on course instance and projects
        /// </summary>
        /// <param name="courseInstanceId">The Id of the courseInstance, gotten from the URL</param>
        /// <param name="model">The projects that should be used to calculate final grade</param>
        /// <returns>Status code, depending on the correctness of the payload</returns>
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
		/// Add a grade for a given person for a given project, or overwrite an old one
		/// </summary>
		/// <param name="courseInstanceID">The Id of the courseInstance, gotten from the URL</param>
		/// <param name="projectID">The Id of the project, gotten from the URL</param>
		/// <param name="viewModel">The grade that is to be added, gotten from the payload</param>
		/// <returns>Status code, depending on the correctness of the payload</returns>
		[HttpPut]
		[Route("project/{projectId:int}/grade")]
		public HttpResponseMessage SetGrade(int courseInstanceID, int projectID, AddGradeViewModel viewModel)
		{
			try
			{
				_service.SetGrade(courseInstanceID, projectID, viewModel);
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

        /// <summary>
        /// gets a grade for a given person for a given project
        /// </summary>
        /// <param name="courseInstanceId">The Id of the courseInstance, gotten from the URL</param>
        /// <param name="projectId">The Id of the project, gotten from the URL</param>
        /// <param name="ssn">The ssn of the student, gotten from the URL</param>
        /// <returns>Status code, depending on the correctness of the payload</returns>
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

        /// <summary>
        /// Gets the final grade
        /// </summary>
        /// <param name="courseInstanceId">The Id of the courseInstance, gotten from the URL</param>
        /// <param name="personSSN">The SSN of the Student, gotten from the URL</param>
        /// <returns>Status code, depending on the correctness of the payload</returns>
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

        /// <summary>
        /// Gets the final grade for all students in course
        /// </summary>
        /// <param name="courseInstanceId">The Id of the courseInstance, gotten from the URL</param>
        /// <returns>Status code, depending on the correctness of the payload</returns>
		[HttpGet]
		[Route("finalGrade/all")]
		public HttpResponseMessage GetAllFinalGrades(int courseInstanceId)
		{
            List<FinalGradeDTO> result;

            try
            {
                result = _service.GetAllFinalGrades(courseInstanceId);
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

        /// <summary>
        /// Gets all grades for a given course
        /// </summary>
        /// <param name="courseInstanceId">The Id of the courseInstance, gotten from the URL</param>
        /// <param name="projectId">The Id of the project, gotten from the URL</param>
        /// <returns>Status code, depending on the correctness of the payload</returns>
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

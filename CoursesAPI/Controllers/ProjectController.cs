﻿using System;
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

		//TODO: Add ProjectViewModel
		[HttpPut]
		[Route("project")]
		public List<int> AddProject(int courseInstanceID)
		{

			return new List<int> { 1,2,3,4,5,6,7};
		}

		//TODO: Add GradeViewModel
		[HttpPut]
		[Route("project/grade")]
		public List<int> AddGrade(int courseInstanceID)
		{

			return new List<int> { 1, 2, 3, 4, 5, 6, 7 };
		}
    }
}

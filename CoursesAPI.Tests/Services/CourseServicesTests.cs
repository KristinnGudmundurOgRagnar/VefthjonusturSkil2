using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoursesAPI.Services;
using CoursesAPI.Services.Services;
using CoursesAPI.Services.Models.Entities;
using CoursesAPI.Tests.MockObjects;
using CoursesAPI.Models;

namespace CoursesAPI.Tests.Services
{

	[TestClass]
	public class CourseServicesTests
	{
		private CoursesServiceProvider _service;
		private int courseInstanceID1 = 1;
		private int courseInstanceID2 = 2;
		private String personSSN1 = "1234567890";
		private String personSSN2 = "0987654321";
		private int projectID1 = 1;
		private int projectID2 = 2;
		private int projectID3 = 3;
		private int invalidCourseInstanceID = 4;

		[TestInitialize]
		public void Setup()
		{
			// TODO: code which will be executed before each test!
			_service = new CoursesServiceProvider(new MockUnitOfWork<MockDataContext>());

			//Add CourseInstances
			List<CourseInstance> courseInstances = new List<CourseInstance>();
			courseInstances.Add(new CourseInstance { ID = courseInstanceID1, SemesterID = "20141" });
			courseInstances.Add(new CourseInstance { ID = courseInstanceID2, SemesterID = "20141" });
			((MockUnitOfWork<MockDataContext>)(_service._uow)).SetRepositoryData<CourseInstance>(courseInstances);


			//Add PersonRegistrations
			List<PersonRegistration> personRegistrations = new List<PersonRegistration>();
			personRegistrations.Add(new PersonRegistration
			{
				ID = 1,
				CourseInstanceId = courseInstanceID1,
				PersonSSN = personSSN1
			});
			personRegistrations.Add(new PersonRegistration
			{
				ID = 2,
				CourseInstanceId = courseInstanceID1,
				PersonSSN = personSSN2
			});
			personRegistrations.Add(new PersonRegistration
			{
				ID = 3,
				CourseInstanceId = courseInstanceID2,
				PersonSSN = personSSN1
			});
			((MockUnitOfWork<MockDataContext>)(_service._uow)).SetRepositoryData<PersonRegistration>(personRegistrations);


			//Add ProjectGroups

			//Add Projects
			List<Project> projects = new List<Project>();
			projects.Add(new Project
			{
				ID = 1,
				Name = "Project 1",
				ProjectGroupId = null,
				CourseInstanceId = courseInstanceID1,
				OnlyHigherThanProjectId = null,
				Weight = 50,
				MinGradeToPassCourse = null
			});
			((MockUnitOfWork<MockDataContext>)(_service._uow)).SetRepositoryData<Project>(projects);


			List<FinalGradeComposition> finalGradeComps = new List<FinalGradeComposition>();
			finalGradeComps.Add(new FinalGradeComposition { ID = 1, CourseInstanceId = 1, ProjectId = 1 });

			((MockUnitOfWork<MockDataContext>)(_service._uow)).SetRepositoryData<FinalGradeComposition>(finalGradeComps);

		}

		/// <summary>
		/// Tests GET /project
		/// </summary>
		[TestMethod]
		public void TestMethod1()
		{
			//Get projects from a course that does not exist
			var theProjects = _service.GetProjectsForCourse(invalidCourseInstanceID);

			Assert.AreEqual(0, theProjects.Count);


			//Get projects from a cource with no projects
			theProjects = _service.GetProjectsForCourse(courseInstanceID2);

			Assert.AreEqual(0, theProjects.Count);


			//Add an invalid project to a valid course
			try
			{
				_service.AddProjectToCourse(courseInstanceID1, new AddProjectViewModel
				{
					Name = "TestProject1",
					ProjectGroupId = 2,
					OnlyHigherThanProjectId = null,
					Weight = 10,
					MinGradeToPassCourse = null
				});
			}
			catch (Exception e)
			{

			}

			theProjects = _service.GetProjectsForCourse(courseInstanceID1);

			Assert.AreEqual(0, theProjects.Count);



			//Add a valid project to an invalid course
			try
			{
				_service.AddProjectToCourse(invalidCourseInstanceID, new AddProjectViewModel
				{
					Name = "TestProject1",
					ProjectGroupId = null,
					OnlyHigherThanProjectId = null,
					Weight = 10,
					MinGradeToPassCourse = null
				});
			}
			catch (Exception e)
			{

			}

			theProjects = _service.GetProjectsForCourse(invalidCourseInstanceID);

			Assert.AreEqual(0, theProjects.Count);


			//Add a valid project to a valid course
			try
			{
				_service.AddProjectToCourse(courseInstanceID1, new AddProjectViewModel
				{
					Name = "TestProject1",
					ProjectGroupId = null,
					OnlyHigherThanProjectId = null,
					Weight = 10,
					MinGradeToPassCourse = null
				});
			}
			catch (Exception e)
			{

			}

			theProjects = _service.GetProjectsForCourse(courseInstanceID1);

			Assert.AreEqual(1, theProjects.Count);
		}
	}
}

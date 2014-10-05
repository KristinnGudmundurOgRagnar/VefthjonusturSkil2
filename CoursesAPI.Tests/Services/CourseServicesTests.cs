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
		private int invalidCourseInstanceID = 6;
		private String personSSN1 = "1234567890";
		private String personSSN2 = "2234567890";
		private String personSSN3 = "3234567890";
		private String invalidPersonSSN = "0000000000";
		private int projectGroupId1 = 1;
		private int invalidProjectID = 666;

		[TestInitialize]
		public void Setup()
		{
			// TODO: code which will be executed before each test!
			_service = new CoursesServiceProvider(new MockUnitOfWork<MockDataContext>());

			#region Add CourseInstances
			List<CourseInstance> courseInstances = new List<CourseInstance>();
			courseInstances.Add(new CourseInstance { ID = 1, SemesterID = "20141" });
			courseInstances.Add(new CourseInstance { ID = 2, SemesterID = "20141" });
			courseInstances.Add(new CourseInstance { ID = 3, SemesterID = "20141" });
			courseInstances.Add(new CourseInstance { ID = 4, SemesterID = "20141" });
			((MockUnitOfWork<MockDataContext>)(_service._uow)).SetRepositoryData<CourseInstance>(courseInstances);
			#endregion


			#region Add PersonRegistrations
			List<PersonRegistration> personRegistrations = new List<PersonRegistration>();
			#region Course 1
			personRegistrations.Add(new PersonRegistration
			{
				ID = 1,
				CourseInstanceId = 1,
				PersonSSN = personSSN1
			});
			personRegistrations.Add(new PersonRegistration
			{
				ID = 2,
				CourseInstanceId = 1,
				PersonSSN = personSSN2
			});
			personRegistrations.Add(new PersonRegistration
			{
				ID = 3,
				CourseInstanceId = 1,
				PersonSSN = personSSN3
			});
			#endregion Course 1


			#region Course 2
			personRegistrations.Add(new PersonRegistration
			{
				ID = 4,
				CourseInstanceId = 2,
				PersonSSN = personSSN1
			});
			#endregion Course 2


			#region Course 3
			personRegistrations.Add(new PersonRegistration
			{
				ID = 5,
				CourseInstanceId = 3,
				PersonSSN = personSSN1
			});
			personRegistrations.Add(new PersonRegistration
			{
				ID = 6,
				CourseInstanceId = 3,
				PersonSSN = personSSN2
			});
			personRegistrations.Add(new PersonRegistration
			{
				ID = 6,
				CourseInstanceId = 3,
				PersonSSN = personSSN3
			});
			#endregion Course 3


			#region Course 4
			personRegistrations.Add(new PersonRegistration
			{
				ID = 7,
				CourseInstanceId = 4,
				PersonSSN = personSSN1
			});
			personRegistrations.Add(new PersonRegistration
			{
				ID = 8,
				CourseInstanceId = 4,
				PersonSSN = personSSN2
			});
			personRegistrations.Add(new PersonRegistration
			{
				ID = 9,
				CourseInstanceId = 4,
				PersonSSN = personSSN3
			});
			#endregion Course 4

			((MockUnitOfWork<MockDataContext>)(_service._uow)).SetRepositoryData<PersonRegistration>(personRegistrations);
			#endregion Add PersonRegistrations


			#region Add ProjectGroups
			List<ProjectGroup> projectGroups = new List<ProjectGroup>();

			projectGroups.Add(new ProjectGroup
			{
				ID = projectGroupId1,
				Name = "Web exams",
				GradedProjectsCount = 2
			});

			((MockUnitOfWork<MockDataContext>)(_service._uow)).SetRepositoryData<ProjectGroup>(projectGroups);
			#endregion Add ProjectGroups


			#region Add Projects
			List<Project> projects = new List<Project>();

			#region Course 1
			//Empty on purpose
			#endregion Course 1


			#region Course 2
			projects.Add(new Project
			{
				ID = 1,
				Name = "Project 1",
				ProjectGroupId = null,
				CourseInstanceId = 2,
				OnlyHigherThanProjectId = null,
				Weight = 50,
				MinGradeToPassCourse = null
			});
			projects.Add(new Project
			{
				ID = 2,
				Name = "Project 2",
				ProjectGroupId = null,
				CourseInstanceId = 2,
				OnlyHigherThanProjectId = null,
				Weight = 25,
				MinGradeToPassCourse = null
			});
			projects.Add(new Project
			{
				ID = 3,
				Name = "Project 3",
				ProjectGroupId = null,
				CourseInstanceId = 2,
				OnlyHigherThanProjectId = null,
				Weight = 25,
				MinGradeToPassCourse = null
			});
			projects.Add(new Project
			{
				ID = 4,
				Name = "Project 4",
				ProjectGroupId = null,
				CourseInstanceId = 2,
				OnlyHigherThanProjectId = null,
				Weight = 50,
				MinGradeToPassCourse = 5
			});
			#endregion Course 2


			#region Course 3
			projects.Add(new Project
			{
				ID = 5,
				Name = "Project 1",
				ProjectGroupId = null,
				CourseInstanceId = 3,
				OnlyHigherThanProjectId = null,
				Weight = 50,
				MinGradeToPassCourse = null
			});
			projects.Add(new Project
			{
				ID = 6,
				Name = "Project 2",
				ProjectGroupId = null,
				CourseInstanceId = 3,
				OnlyHigherThanProjectId = null,
				Weight = 25,
				MinGradeToPassCourse = null
			});
			projects.Add(new Project
			{
				ID = 7,
				Name = "Project 3",
				ProjectGroupId = null,
				CourseInstanceId = 3,
				OnlyHigherThanProjectId = null,
				Weight = 25,
				MinGradeToPassCourse = null
			});
			projects.Add(new Project
			{
				ID = 8,
				Name = "Project 4",
				ProjectGroupId = null,
				CourseInstanceId = 3,
				OnlyHigherThanProjectId = null,
				Weight = 50,
				MinGradeToPassCourse = 5
			});
			#endregion Course 3


			#region Course 4
			projects.Add(new Project
			{
				ID = 9,
				Name = "Project 1",
				ProjectGroupId = null,
				CourseInstanceId = 4,
				OnlyHigherThanProjectId = 10,
				Weight = 10,
				MinGradeToPassCourse = null
			});
			projects.Add(new Project
			{
				ID = 10,
				Name = "Project 2",
				ProjectGroupId = null,
				CourseInstanceId = 4,
				OnlyHigherThanProjectId = null,
				Weight = 60,
				MinGradeToPassCourse = null
			});
			projects.Add(new Project
			{
				ID = 11,
				Name = "Project 3",
				ProjectGroupId = null,
				CourseInstanceId = 4,
				OnlyHigherThanProjectId = null,
				Weight = 20,
				MinGradeToPassCourse = null
			});

			//Three projects in the same project group
			projects.Add(new Project
			{
				ID = 12,
				Name = "Web Exam 1",
				ProjectGroupId = projectGroupId1,
				CourseInstanceId = 4,
				OnlyHigherThanProjectId = null,
				Weight = 5,
				MinGradeToPassCourse = null
			});
			projects.Add(new Project
			{
				ID = 13,
				Name = "Web Exam 2",
				ProjectGroupId = projectGroupId1,
				CourseInstanceId = 4,
				OnlyHigherThanProjectId = null,
				Weight = 5,
				MinGradeToPassCourse = null
			});
			projects.Add(new Project
			{
				ID = 14,
				Name = "Web Exam 3",
				ProjectGroupId = projectGroupId1,
				CourseInstanceId = 4,
				OnlyHigherThanProjectId = null,
				Weight = 5,
				MinGradeToPassCourse = null
			});

			#endregion Course 4

			((MockUnitOfWork<MockDataContext>)(_service._uow)).SetRepositoryData<Project>(projects);
			#endregion Add Projects


			#region Add Grades

			List<Grade> grades = new List<Grade>();
			#region Person 1
			grades.Add(new Grade
			{
				ID = 1,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 1
			});
			grades.Add(new Grade
			{
				ID = 2,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 2
			});
			grades.Add(new Grade
			{
				ID = 3,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 3
			});
			grades.Add(new Grade
			{
				ID = 4,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 4
			});
			grades.Add(new Grade
			{
				ID = 5,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 5
			});
			grades.Add(new Grade
			{
				ID = 6,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 6
			});
			grades.Add(new Grade
			{
				ID = 7,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 7
			});
			grades.Add(new Grade
			{
				ID = 8,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 8
			});
			grades.Add(new Grade
			{
				ID = 9,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 9
			});
			grades.Add(new Grade
			{
				ID = 10,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 10
			});
			grades.Add(new Grade
			{
				ID = 11,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 11
			});
			grades.Add(new Grade
			{
				ID = 12,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 12
			});
			grades.Add(new Grade
			{
				ID = 13,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 13
			});
			grades.Add(new Grade
			{
				ID = 14,
				PersonSSN = personSSN1,
				GradeValue = 0,
				ProjectId = 14
			});

			#endregion Person 1


			#region Person 2
			grades.Add(new Grade
			{
				ID = 15,
				PersonSSN = personSSN2,
				GradeValue = 100,
				ProjectId = 5
			});
			grades.Add(new Grade
			{
				ID = 16,
				PersonSSN = personSSN2,
				GradeValue = 100,
				ProjectId = 6
			});
			grades.Add(new Grade
			{
				ID = 17,
				PersonSSN = personSSN2,
				GradeValue = 100,
				ProjectId = 7
			});
			grades.Add(new Grade
			{
				ID = 18,
				PersonSSN = personSSN2,
				GradeValue = 100,
				ProjectId = 8
			});
			grades.Add(new Grade
			{
				ID = 19,
				PersonSSN = personSSN2,
				GradeValue = 100,
				ProjectId = 9
			});
			grades.Add(new Grade
			{
				ID = 20,
				PersonSSN = personSSN2,
				GradeValue = 100,
				ProjectId = 10
			});
			grades.Add(new Grade
			{
				ID = 21,
				PersonSSN = personSSN2,
				GradeValue = 100,
				ProjectId = 11
			});
			grades.Add(new Grade
			{
				ID = 22,
				PersonSSN = personSSN2,
				GradeValue = 100,
				ProjectId = 12
			});
			grades.Add(new Grade
			{
				ID = 23,
				PersonSSN = personSSN2,
				GradeValue = 100,
				ProjectId = 13
			});
			grades.Add(new Grade
			{
				ID = 24,
				PersonSSN = personSSN2,
				GradeValue = 100,
				ProjectId = 14
			});

			#endregion Person 2

			((MockUnitOfWork<MockDataContext>)(_service._uow)).SetRepositoryData<Grade>(grades);
			#endregion Add Grades


			#region Add FinalGradeCompositions
			List<FinalGradeComposition> finalGradeComps = new List<FinalGradeComposition>();
			#region Course 1
			//Empty on purpose
			#endregion Course 1


			#region Course 2
			finalGradeComps.Add(new FinalGradeComposition
			{
				ID = 1,
				CourseInstanceId = 2,
				ProjectId = 1
			});
			finalGradeComps.Add(new FinalGradeComposition
			{
				ID = 2,
				CourseInstanceId = 2,
				ProjectId = 2
			});
			finalGradeComps.Add(new FinalGradeComposition
			{
				ID = 3,
				CourseInstanceId = 2,
				ProjectId = 3
			});
			#endregion course 2


			#region Course 3
			finalGradeComps.Add(new FinalGradeComposition
			{
				ID = 4,
				CourseInstanceId = 3,
				ProjectId = 5
			});
			finalGradeComps.Add(new FinalGradeComposition
			{
				ID = 5,
				CourseInstanceId = 3,
				ProjectId = 6
			});
			finalGradeComps.Add(new FinalGradeComposition
			{
				ID = 6,
				CourseInstanceId = 3,
				ProjectId = 7
			});
			#endregion Course 3


			#region Course 4
			finalGradeComps.Add(new FinalGradeComposition
			{
				ID = 7,
				CourseInstanceId = 4,
				ProjectId = 9
			});
			finalGradeComps.Add(new FinalGradeComposition
			{
				ID = 8,
				CourseInstanceId = 4,
				ProjectId = 10
			});
			finalGradeComps.Add(new FinalGradeComposition
			{
				ID = 9,
				CourseInstanceId = 4,
				ProjectId = 11
			});
			finalGradeComps.Add(new FinalGradeComposition
			{
				ID = 10,
				CourseInstanceId = 4,
				ProjectId = 12
			});
			finalGradeComps.Add(new FinalGradeComposition
			{
				ID = 11,
				CourseInstanceId = 4,
				ProjectId = 13
			});
			finalGradeComps.Add(new FinalGradeComposition
			{
				ID = 12,
				CourseInstanceId = 4,
				ProjectId = 14
			});
			#endregion Course 4
			((MockUnitOfWork<MockDataContext>)(_service._uow)).SetRepositoryData<FinalGradeComposition>(finalGradeComps);
			#endregion Add FinalGradeCompositions
		}

		/// <summary>
		/// Tests GET /project
		/// </summary>
		[TestMethod]
		public void TestGetProjects()
		{
			//Get projects from a course that does not exist
			var theProjects = _service.GetProjectsForCourse(invalidCourseInstanceID);

			Assert.AreEqual(0, theProjects.Count);


			//Get projects from a cource with no projects
			theProjects = _service.GetProjectsForCourse(1);

			Assert.AreEqual(0, theProjects.Count);


			//Add an invalid project to a valid course
			try
			{
				_service.AddProjectToCourse(1, new AddProjectViewModel
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

			theProjects = _service.GetProjectsForCourse(1);

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
				_service.AddProjectToCourse(1, new AddProjectViewModel
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

			theProjects = _service.GetProjectsForCourse(1);

			Assert.AreEqual(1, theProjects.Count);
		}

		/// <summary>
		/// Tests GET /finalGrade/personSSN
		/// Tests GET /finalGrade/all
		/// </summary>
		[TestMethod]
		public void TestGetFinalGrade()
		{
			FinalGradeDTO finalGrade = null;
			bool exceptionThrown = false;

			//Try to get a final grade from a course with no projects
			try
			{
				finalGrade = _service.GetFinalGradeForOneStudent(1, personSSN1);
			}
			catch (Exception e)
			{
				exceptionThrown = true;
			}

			Assert.IsTrue(exceptionThrown);
			exceptionThrown = false;


			//Try to get a final grade from a course with projects and a student with all 0 for grades
			try
			{
				finalGrade = _service.GetFinalGradeForOneStudent(2, personSSN1);
			}
			catch (Exception e)
			{
				exceptionThrown = true;
			}

			Assert.IsFalse(exceptionThrown);
			Assert.AreEqual(0, finalGrade.Grade);
			Assert.AreEqual(1, finalGrade.NumberOfStudents);
			Assert.AreEqual(100, finalGrade.PercentageComplete);
			Assert.AreEqual(1, finalGrade.PositionLower);
			Assert.AreEqual(1, finalGrade.PositionUpper);
			Assert.AreEqual(personSSN1, finalGrade.PersonSSN);
			Assert.AreEqual("OK", finalGrade.Status);
			exceptionThrown = false;


			//Try to get a final grade from a course with projects and a student who is not in the course
			try
			{
				finalGrade = _service.GetFinalGradeForOneStudent(2, personSSN2);
			}
			catch (Exception e)
			{
				exceptionThrown = true;
			}

			Assert.IsTrue(exceptionThrown);
			exceptionThrown = false;


			//Try to get a final grade from a course with projects and a student with all 10 for grades
			try
			{
				finalGrade = _service.GetFinalGradeForOneStudent(3, personSSN2);
			}
			catch (Exception e)
			{
				exceptionThrown = true;
			}

			Assert.IsFalse(exceptionThrown);
			Assert.AreEqual(10, finalGrade.Grade);
			Assert.AreEqual(3, finalGrade.NumberOfStudents);
			Assert.AreEqual(100, finalGrade.PercentageComplete);
			Assert.AreEqual(1, finalGrade.PositionLower);
			Assert.AreEqual(1, finalGrade.PositionUpper);
			Assert.AreEqual(personSSN2, finalGrade.PersonSSN);
			Assert.AreEqual("OK", finalGrade.Status);
			exceptionThrown = false;


			//Try to get a final grade from a course with projects and a student who has not finished any projects
			try
			{
				finalGrade = _service.GetFinalGradeForOneStudent(3, personSSN3);
			}
			catch (Exception e)
			{
				exceptionThrown = true;
			}

			Assert.IsFalse(exceptionThrown);
			Assert.AreEqual(0, finalGrade.Grade);
			Assert.AreEqual(3, finalGrade.NumberOfStudents);
			Assert.AreEqual(0, finalGrade.PercentageComplete);
			Assert.AreEqual(3, finalGrade.PositionLower);
			Assert.AreEqual(2, finalGrade.PositionUpper);
			Assert.AreEqual(personSSN3, finalGrade.PersonSSN);
			Assert.AreEqual("OK", finalGrade.Status);
			exceptionThrown = false;


			//Try to get a final grade from a course with complicated projects and a student who has 10 in everything
			try
			{
				finalGrade = _service.GetFinalGradeForOneStudent(4, personSSN2);
			}
			catch (Exception e)
			{
				exceptionThrown = true;
			}

			Assert.IsFalse(exceptionThrown);
			Assert.AreEqual(10, finalGrade.Grade);
			Assert.AreEqual(3, finalGrade.NumberOfStudents);
			Assert.AreEqual(100, finalGrade.PercentageComplete);
			Assert.AreEqual(1, finalGrade.PositionLower);
			Assert.AreEqual(1, finalGrade.PositionUpper);
			Assert.AreEqual(personSSN2, finalGrade.PersonSSN);
			Assert.AreEqual("OK", finalGrade.Status);
			exceptionThrown = false;


			//Try to get a final grade from a course with complicated projects and a student who has 0 in everything
			try
			{
				finalGrade = _service.GetFinalGradeForOneStudent(4, personSSN1);
			}
			catch (Exception e)
			{
				exceptionThrown = true;
			}

			Assert.IsFalse(exceptionThrown);
			Assert.AreEqual(0, finalGrade.Grade);
			Assert.AreEqual(3, finalGrade.NumberOfStudents);
			Assert.AreEqual(100, finalGrade.PercentageComplete);
			Assert.AreEqual(3, finalGrade.PositionLower);
			Assert.AreEqual(2, finalGrade.PositionUpper);
			Assert.AreEqual(personSSN1, finalGrade.PersonSSN);
			Assert.AreEqual("OK", finalGrade.Status);
			exceptionThrown = false;

			//Get all the final grades from a course with students who have both all grades and no grades
			List<FinalGradeDTO> allFinalGrades = null;
			try
			{
				allFinalGrades = _service.GetAllFinalGrades(3);
			}
			catch(Exception e){
				exceptionThrown = true;
			}

			Assert.IsFalse(exceptionThrown);
			Assert.AreEqual(3, allFinalGrades.Count);

			List<double> temp = new List<double>();
			foreach(FinalGradeDTO f in allFinalGrades){
				temp.Add(f.Grade);
			}

			CollectionAssert.Contains(temp, 0.0);
			CollectionAssert.Contains(temp, 10.0);
			CollectionAssert.DoesNotContain(temp, null);


			List<int> tempIntList = new List<int>();
			foreach(FinalGradeDTO f in allFinalGrades){
				tempIntList.Add(f.PercentageComplete);
			}

			CollectionAssert.Contains(tempIntList, 0);
			CollectionAssert.Contains(tempIntList, 100);
			CollectionAssert.DoesNotContain(tempIntList, null);
		}

		/// <summary>
		/// Tests GET /project/{projectId}/grade
		/// Tests PUT /project/{projectId}/grade
		/// </summary>
		[TestMethod]
		public void TestAddGrade()
		{
			bool exceptionThrown = false;
			GradeDTO currentGrade = null;

			//Try to add a grade for a course in which the student is not
			try
			{
				_service.SetGrade(2, 1, new AddGradeViewModel
					{
						Grade = 77,
						PersonSSN = personSSN3
					});
			}
			catch(Exception e)
			{
				exceptionThrown = true;
			}

			Assert.IsTrue(exceptionThrown);
			exceptionThrown = false;


			//Try to add a grade for a course in which the student is, but for a project outside of the course
			try
			{
				_service.SetGrade(2, 5, new AddGradeViewModel
				{
					Grade = 77,
					PersonSSN = personSSN3
				});
			}
			catch (Exception e)
			{
				exceptionThrown = true;
			}

			Assert.IsTrue(exceptionThrown);
			exceptionThrown = false;


			//Try to add a valid grade
			try
			{
				_service.SetGrade(3, 5, new AddGradeViewModel
				{
					Grade = 77,
					PersonSSN = personSSN3
				});
			}
			catch (Exception e)
			{
				exceptionThrown = true;
			}

			Assert.IsFalse(exceptionThrown);
			exceptionThrown = false;

			//Try to get the newly added grade
			try
			{
				currentGrade = _service.GetProjectGrade(3, 5, personSSN3);
			}
			catch(Exception e){
				exceptionThrown = true;
			}

			Assert.IsFalse(exceptionThrown);
			Assert.AreEqual(77, currentGrade.Grade);
			Assert.AreEqual(3, currentGrade.NumberOfStudents);
			Assert.AreEqual(2, currentGrade.PositionUpper);
			Assert.AreEqual(2, currentGrade.PositionLower);
			Assert.AreEqual(personSSN3, currentGrade.SSN);
			exceptionThrown = false;


			//Try to change the newly added grade
			try
			{
				_service.SetGrade(3, 5, new AddGradeViewModel
				{
					Grade = 50,
					PersonSSN = personSSN3
				});
			}
			catch(Exception e){
				exceptionThrown = true;
			}


			Assert.IsFalse(exceptionThrown);
			exceptionThrown = false;


			//Try to access the newly updated grade
			try
			{
				currentGrade = _service.GetProjectGrade(3, 5, personSSN3);
			}
			catch (Exception e)
			{
				exceptionThrown = true;
			}

			Assert.IsFalse(exceptionThrown);
			Assert.AreEqual(50, currentGrade.Grade);
			Assert.AreEqual(3, currentGrade.NumberOfStudents);
			Assert.AreEqual(2, currentGrade.PositionUpper);
			Assert.AreEqual(2, currentGrade.PositionLower);
			Assert.AreEqual(personSSN3, currentGrade.SSN);
			exceptionThrown = false;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesAPI.Services.Models.Entities
{
    /// <summary>
    /// Projects object for holding information about the project
    /// </summary>
	[Table("Projects")]
	public class Project
	{

		/// <summary>
		/// A database-generated ID of the project
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// The name of the project
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// The ID of the ProjectGroup that contains this Project, if any
		/// </summary>
		public int? ProjectGroupId { get; set; }

		/// <summary>
		/// The course int which this Project is
		/// </summary>
		public int CourseInstanceId { get; set; }

		/// <summary>
		/// Used to implement that some projects are for bonus credit
		/// </summary>
		public int? OnlyHigherThanProjectId { get; set; }

		/// <summary>
		/// The weight percentage of the project
		/// </summary>
		public int Weight { get; set; }

		/// <summary>
		/// The minimum grade needed to pass the course in which this project is. Optional
		/// </summary>
		public int? MinGradeToPassCourse { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesAPI.Services.Models.Entities
{
	[Table("Grades")]
	class Grade
	{
		/// <summary>
		/// A database-generated ID of the grade
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// The id of the project with which this grade is associated
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// The grade (0 - 100)
		/// </summary>
		public int? Grade { get; set; }

		/// <summary>
		/// The SSN of the person whose grade this is
		/// </summary>
		public String PersonSSN { get; set; }
	}
}

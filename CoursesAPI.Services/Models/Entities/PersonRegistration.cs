using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesAPI.Services.Models.Entities
{
	[Table("PersonRegistrations")]
	public class PersonRegistration
	{
		/// <summary>
		/// A database-generated Id of the registration
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// The id of the CourseInstance
		/// </summary>
		public int CourseInstanceId { get; set; }

		/// <summary>
		/// The SSN of the person
		/// </summary>
		public String PersonSSN { get; set; }
	}
}

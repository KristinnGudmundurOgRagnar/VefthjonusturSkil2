using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesAPI.Services.Models.Entities
{
    /// <summary>
    /// Used to connect all projects to a course for grading purpose
    /// </summary>
	[Table("FinalGradeCompositions")]
	public class FinalGradeComposition
	{
		/// <summary>
		/// The database-generated Id of the record
		/// </summary>
		public int ID { get; set; }
		/// <summary>
		/// The Id of the courseInstance
		/// </summary>
		public int CourseInstanceId { get; set; }
		/// <summary>
		/// The Id of the project
		/// </summary>
		public int ProjectId { get; set; }
	}
}

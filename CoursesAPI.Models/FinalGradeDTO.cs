using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Models
{
	public class FinalGradeDTO
	{
		/// <summary>
		/// The accumulated grade
		/// </summary>
		public double Grade { get; set; }
		/// <summary>
		/// The percentage of the final grade that the student has earned
		/// </summary>
		public int PercentageComplete { get; set; }
		/// <summary>
		/// The SSN of the student
		/// </summary>
		public String PersonSSN { get; set; }
		/// <summary>
		/// The upper bounds of the position of the student
		/// </summary>
		public int? PositionUpper { get; set; }
		/// <summary>
		/// The lower bounds of the position of the student
		/// </summary>
		public int? PositionLower { get; set; }
		/// <summary>
		/// The total number of students in the course
		/// </summary>
		public int NumberOfStudents { get; set; }
		/// <summary>
		/// The status of the final grade
		/// Whether or not the student has failed or not 
		/// </summary>
		public String Status { get; set; }
	}
}

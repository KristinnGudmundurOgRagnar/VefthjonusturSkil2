using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Models
{
	public class GradeDTO
	{
        /// <summary>
        /// The SSN of the student
        /// </summary>
        public String SSN { get; set; }
		/// <summary>
		/// The grade of the student
		/// </summary>
		public int? Grade { get; set; }
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
	}
}

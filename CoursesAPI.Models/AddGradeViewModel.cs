using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Models
{
    /// <summary>
    /// ViewModel for adding grade
    /// </summary>
	public class AddGradeViewModel
	{
        /// <summary>
        /// The Grade for the project
        /// </summary>
		public int? Grade       { get; set; }

        /// <summary>
        /// the SSN of the student to be graded
        /// </summary>
		public String PersonSSN { get; set; }
	}
}

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
	}
}

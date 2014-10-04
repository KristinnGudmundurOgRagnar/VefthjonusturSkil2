using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Models
{
	public class AddFinalGradeCompositionViewModel
	{
		/// <summary>
		/// A list of the ids of the projects that should
		/// be used to calculate the final grade
		/// </summary>
		public List<int> Projects { get; set; }
	}
}

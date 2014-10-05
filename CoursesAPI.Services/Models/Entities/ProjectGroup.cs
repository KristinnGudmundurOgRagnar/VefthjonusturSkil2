using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesAPI.Services.Models.Entities
{
    /// <summary>
    /// Some projects may be in group so they can be evaluated together to get
    /// a final grade based on e.g. top three grades
    /// </summary>
	[Table("ProjectGroups")]
	public class ProjectGroup
	{
		/// <summary>
		/// A database generated Id of the project group
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// The name of the project group
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// The number of projects in this group that will be used to calculate a final grade
		/// </summary>
		public int GradedProjectsCount { get; set; }
	}
}

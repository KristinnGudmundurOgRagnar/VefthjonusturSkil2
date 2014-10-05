using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Models
{
    /// <summary>
    /// ViewModel for a project
    /// </summary>
    public class AddProjectViewModel
    {
        /// <summary>
        /// Name of Course
        /// </summary>
        public String Name                      { get; set; }

		/// <summary>
		/// Id of projectgroup
		/// </summary>
        public int? ProjectGroupId              { get; set; }

        /// <summary>
		/// Id of onlyhigherthan
        /// </summary>
        public int? OnlyHigherThanProjectId     { get; set; }

		/// <summary>
		/// weight of project
		/// </summary>
        public int Weight                       { get; set; }

        /// <summary>
		/// Minimum grade to pass course
        /// </summary>
        public int? MinGradeToPassCourse        { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Models
{
    public class AddProjectViewModel
    {
        /// <summary>
        /// Name of Course
        /// </summary>
        public String Name;
		/// <summary>
		/// Id of projectgroup
		/// </summary>
        public int? ProjectGroupId;
        /// <summary>
		/// Id of onlyhigherthan
        /// </summary>
		public int ? OnlyHigherThanProjectId;
		/// <summary>
		/// weight of project
		/// </summary>
        public int Weight;
        /// <summary>
		/// Minimum grade to pass course
        /// </summary>
		public int ? MinGradeToPassCourse;
    }
}

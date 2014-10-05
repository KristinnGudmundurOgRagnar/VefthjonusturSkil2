using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Models
{
    /// <summary>
    /// ViewModel for adding Project group
    /// </summary>
    public class AddProjectGroupViewModel
    {
        /// <summary>
        /// Name of ProjectGroup
        /// </summary>
        public String Name              { get; set; }

        /// <summary>
        /// Number of top projects to be evaluated
        /// </summary>
        public int? GradedProjectsCount { get; set; }
    }
}

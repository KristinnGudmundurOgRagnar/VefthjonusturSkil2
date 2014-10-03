using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Models
{
    public class AddProjectGroupViewModel
    {
        //Name of ProjectGroup
        public String Name { get; set; }
        //number of top projects to be evaluated
        public int? GradedProjectsCount { get; set; }

    }
}

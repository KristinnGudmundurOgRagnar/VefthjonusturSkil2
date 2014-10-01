using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Models
{
    public class AddProjectViewModel
    {
        //Name of Course
        public String Name;
        //Id of projectgroup
        public int ProjectGroupId;
        //Id of onlyhigherthan
        public int OnlyHigherThanProjectId;
        //weight of project
        public int Weight;
        //Minimum grade to pass course
        public int MinGradeToPassCourse;
    }
}

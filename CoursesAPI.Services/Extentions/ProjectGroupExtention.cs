using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoursesAPI.Services.Models.Entities;
using CoursesAPI.Services.DataAccess;

namespace CoursesAPI.Services.Extensions
{
    public static class ProjectGroupExtention
    {
        public static ProjectGroup GetGroupByID(this IRepository<ProjectGroup> repo, int id)
        {
            var group = repo.All().SingleOrDefault(c => c.ID == id);

            if (group == null)
            {
                throw new KeyNotFoundException("No projectGroup found with this ID");
            }
            return group;
        }
    }
}

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
        /// <summary>
        /// An extention method to get all Project-Groups for the given Id
        /// </summary>
        /// <param name="repo">the repository to search in</param>
        /// <param name="id">the id to look for</param>
        /// <returns>ProjectGroup</returns>
        public static ProjectGroup GetGroupByID(this IRepository<ProjectGroup> repo, int id)
        {
            try
            {
                var group = repo.All().SingleOrDefault(c => c.ID == id);

                if (group == null)
                {
                    throw new KeyNotFoundException("No projectGroup found with this ID: " + id);
                }
                return group;
            }
            catch (Exception)
            {
                throw new Exception("Found two Project-Groups with same id: " + id);
            }
        }
    }
}

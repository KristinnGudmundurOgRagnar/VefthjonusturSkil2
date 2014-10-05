using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoursesAPI.Services.Models.Entities;
using CoursesAPI.Services.DataAccess;

namespace CoursesAPI.Services.Extensions
{
    public static class ProjectExtention
    {
        public static Project GetProjectByID(this IRepository<Project> repo, int id)
        {
            try
            {
                var pro = repo.All().SingleOrDefault(c => c.ID == id);

                if (pro == null)
                {
                    throw new KeyNotFoundException("No project found with this ID: " + id);
                }
                return pro;
            }
            catch (Exception)
            {
                throw new Exception("Found two Projects with same id: " + id);
            }


        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoursesAPI.Services.Models.Entities;
using CoursesAPI.Services.DataAccess;

namespace CoursesAPI.Services.Extensions
{
    public static class CourseInstanceExtentions
    {
        public static CourseInstance GetCourseByID(this IRepository<CourseInstance> repo, int id)
        {
            var course = repo.All().SingleOrDefault(c => c.ID == id);

            if (course == null)
            {
                throw new ArgumentException("Invalid course instance ID");
            }
            return course;
        }
    }
}
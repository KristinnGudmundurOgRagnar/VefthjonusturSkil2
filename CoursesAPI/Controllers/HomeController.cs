using CoursesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoursesAPI.Controllers
{
	public class HomeController : Controller
	{

        /// <summary>
        /// A dummy controller
        /// </summary>
        /// <returns></returns>
		public ActionResult Index()
		{
			ViewBag.Title = "Home Page";

			return View();
		}

        /// <summary>
        /// A dummy controller
        /// </summary>
        /// <returns></returns>
        public ActionResult Post()
        {
            return View(new LanguageViewModel());
        }

        /// <summary>
        /// A dummy controller
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete()
        {
            return View(new LanguageViewModel());
        }

	}
}

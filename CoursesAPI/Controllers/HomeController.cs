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
		public ActionResult Index()
		{
			ViewBag.Title = "Home Page";

			return View();
		}

        //Action for testing the post method in Language API controller
        public ActionResult Post()
        {
            return View(new LanguageViewModel());
        }

        public ActionResult Delete()
        {
            return View(new LanguageViewModel());
        }

	}
}

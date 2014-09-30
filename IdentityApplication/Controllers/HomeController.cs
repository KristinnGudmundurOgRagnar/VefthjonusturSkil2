using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentityApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Claims()
        {
            ViewBag.Message = "Claims";

            IEnumerable<Claim> claims = System.Security.Claims.ClaimsPrincipal.Current.Claims;

            return View();
        }

        public ActionResult Signout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }

        public async Task<ActionResult> CallService()
        {

            HttpClient client = new HttpClient
            {
                //BaseAddress = new Uri("http://localhost:2727/")
                BaseAddress = new Uri("http://localhost:64212/")
            };

            ClaimsPrincipal id = System.Security.Claims.ClaimsPrincipal.Current;

            if (id.FindFirst("access_token") != null)
            {
                String accessToken = id.FindFirst("access_token").Value;
                //client.SetBearerToken(accessToken);
                client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", accessToken));


            }

            System.Net.Http.HttpResponseMessage response = await client.GetAsync("Identity");


            if (response.StatusCode == HttpStatusCode.OK)
            {
                String json = response.Content.ReadAsStringAsync().Result;
                ViewBag.Message = "Authorized";
                ViewBag.Result = JArray.Parse(json);
            }
            else
            {
                var result = response.StatusCode.ToString();
                ViewBag.Message = result;
                ViewBag.Result = new JArray();
            }

            return View();
        }

        public async Task<ActionResult> CallService2()
        {

            HttpClient client = new HttpClient
            {
                //BaseAddress = new Uri("http://localhost:2727/")
                BaseAddress = new Uri("http://localhost:64212/")
            };

            ClaimsPrincipal id = System.Security.Claims.ClaimsPrincipal.Current;

            if (id.FindFirst("access_token") != null)
            {
                String accessToken = id.FindFirst("access_token").Value;
                //client.SetBearerToken(accessToken);
                client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", accessToken));


            }

            System.Net.Http.HttpResponseMessage response = await client.GetAsync("Identity/?id=1");


            if (response.StatusCode == HttpStatusCode.OK)
            {
                String json = response.Content.ReadAsStringAsync().Result;
                ViewBag.Message = "Authorized";
                ViewBag.Result = JArray.Parse(json);
            }
            else
            {
                var result = response.StatusCode.ToString();
                ViewBag.Message = result;
                ViewBag.Result = new JArray();
            }

            return View();
        }
    }
}
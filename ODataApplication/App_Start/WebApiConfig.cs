using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using ODataApplication.Models;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace ODataApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            //config.MapHttpAttributeRoutes();

            /*config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );*/

            // New code:
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            config.MapODataServiceRoute(routeName: "ODataRoute",routePrefix: "odata",model: builder.GetEdmModel());

        }
    }
}

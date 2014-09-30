using CoursesAPI.Filters;
using CoursesAPI.Loggers;
using CoursesAPI.Tracers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Tracing;

namespace CoursesAPI
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

            SystemDiagnosticsTraceWriter traceWriter = config.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = true;
            traceWriter.MinimumLevel = TraceLevel.Debug;

            //Replace default tracer
            config.Services.Replace(typeof(ITraceWriter), new CourseAPITracer());

			// Web API routes
			config.MapHttpAttributeRoutes();

            // Add a filter for exceptions
            config.Filters.Add(new AppExceptionFilter());

            // Add logger
            config.Services.Add(typeof(IExceptionLogger), new TraceExceptionLogger());            
            config.Services.Add(typeof (IExceptionLogger), new RaygunExceptionLogger());

            // add handler for exceptions
            config.Services.Replace(typeof(IExceptionHandler), new CourseAPIExceptionHandler());

            // Set Language handler to detect and set localized languages
            config.MessageHandlers.Add(new LanguageMessageHandler());

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}

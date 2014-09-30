using Griffin.MvcContrib.Localization;
using Griffin.MvcContrib.Localization.ValidationMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CoursesAPI
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

            var stringProvider = new ResourceStringProvider(Resources.Resources.ResourceManager);
            System.Web.Mvc.ModelMetadataProviders.Current = new LocalizedModelMetadataProvider(stringProvider);

            ValidationMessageProviders.Clear();
            ValidationMessageProviders.Add(new GriffinStringsProvider(stringProvider)); // the rest
            ValidationMessageProviders.Add(new MvcDataSource()); //mvc attributes
            ValidationMessageProviders.Add(new DataAnnotationDefaultStrings()); //data annotation attributes

            System.Web.Mvc.ModelValidatorProviders.Providers.Clear();
            System.Web.Mvc.ModelValidatorProviders.Providers.Add(new LocalizedModelValidatorProvider());
		}

        
	}
}

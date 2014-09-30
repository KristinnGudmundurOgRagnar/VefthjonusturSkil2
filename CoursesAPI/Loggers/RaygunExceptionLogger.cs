using Mindscape.Raygun4Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace CoursesAPI.Loggers
{
    public class RaygunExceptionLogger : ExceptionLogger
    {
        private readonly string _apiKey;

        public RaygunExceptionLogger()
            : this(ConfigurationManager.AppSettings["RaygunAppKey"])
        { }

        public RaygunExceptionLogger(string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException("apiKey");
            _apiKey = apiKey;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            //Create instance of Logging API and log the exception with API
            //RaygunClient _client = new RaygunClient("W6mFxHVXRvLBIyXEPvjiGA==");
            //_client.Send(context.Exception);
        }

        /*public async override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                var message = new WebApiRaygunMessage
                {
                    Details =
                    {
                        Request = new WebApiRaygunRequestMessage(context.Request),
                        Error = new WebApiRaygunErrorMessage(context.Exception),
                        Environment = new WebApiRaygunEnvironmentMessage(),
                        User = new WebApiRaygunIdentifierMessage(context.RequestContext != null && context.RequestContext.Principal != null ? context.RequestContext.Principal.Identity.Name : "Anonymous"),
                        Client = new WebApiRaygunClientMessage()
                    }
                };

                var client = new HttpClient();
                var msg = new HttpRequestMessage(HttpMethod.Post, "https://api.raygun.io/entries");
                msg.Headers.Add("X-ApiKey", _apiKey);
                msg.Content = new ObjectContent<WebApiRaygunMessage>(message, new JsonMediaTypeFormatter());

                await client.SendAsync(msg, cancellationToken);
            }
        }*/
    }
}
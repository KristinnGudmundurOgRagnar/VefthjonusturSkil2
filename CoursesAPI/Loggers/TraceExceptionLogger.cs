using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace CoursesAPI.Loggers
{
    public class TraceExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            //System.Diagnostics.Trace.TraceError(context.ExceptionContext.Exception.ToString());
        }
    }
}
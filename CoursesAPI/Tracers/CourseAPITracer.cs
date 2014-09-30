using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.Tracing;

namespace CoursesAPI.Tracers
{
    public class CourseAPITracer : ITraceWriter
    {
        public void Trace(HttpRequestMessage request, string category, TraceLevel level,
            Action<TraceRecord> traceAction)
        {
            TraceRecord rec = new TraceRecord(request, category, level);
            traceAction(rec);
            WriteTrace(rec);
        }

        protected void WriteTrace(TraceRecord rec)
        {
            //Write to output
            var message = string.Format("{0};{1};{2}", rec.Operator, rec.Operation, rec.Message);            
            System.Diagnostics.Trace.WriteLine(message, rec.Category);
            //Write to file
            string path = HttpContext.Current.Server.MapPath("~/Logs/MyTestLog.txt");
            System.IO.File.AppendAllText(path, rec.Status + " - " + rec.Message + "\r\n");
        }
    }
}
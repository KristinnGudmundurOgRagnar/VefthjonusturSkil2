using CoursesAPI.Services.Exceptions;
using Mindscape.Raygun4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace CoursesAPI.Filters
{
    /// <summary>
    /// An attempt to write a generic exception filter, which would ensure..
    /// </summary>
    public class AppExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);

            //Create instance of Logging API and log the exception with API
            //RaygunClient _client = new RaygunClient("W6mFxHVXRvLBIyXEPvjiGA==");            
            //_client.Send(actionExecutedContext.Exception);

            if (actionExecutedContext.Exception is CoursesAPIObjectNotFoundException)
            {
                var ex = actionExecutedContext.Exception as CoursesAPIObjectNotFoundException;
                HttpError error = new HttpError();

                var validationResults = ex.GetValidationResults();
                if (validationResults.Count > 0 && validationResults.Where(x => x.ErrorMessage == ErrorCodes.LanguageDoesNotExist).FirstOrDefault() != null)
                {
                    error.Add("LanguageDoesNotExist", Resources.Resources.LanguageDoesNotExist);
                }
                else
                {                     
                    error.Add("CoursesAPIObjectNotFoundException", actionExecutedContext.Exception.Message);
                }

                actionExecutedContext.Response = actionExecutedContext.ActionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);

            }

            else if (actionExecutedContext.Exception is CoursesAPIValidationException)
            {
                /*var ex = actionExecutedContext.Exception as CoursesAPIValidationException;

                HttpError error = new HttpError();

                var validationResults = ex.GetValidationResults();

                if (validationResults.Count > 0 && validationResults.Where(x => x.ErrorMessage == ErrorCodes.IdDoesNotExistException).FirstOrDefault() != null)
                {
                    error.Add("CoursesAPIValidationException", Resources.Resources.IdDoesNotExistException);
                }
                else
                { 
                    ModelStateDictionary modelState = actionExecutedContext.ActionContext.ModelState;
                    error = GetErrors(modelState, true);                    
                }

                actionExecutedContext.Response = actionExecutedContext.ActionContext.Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, error);*/
            }

            // This one should be the last one in this if/else chain,
            // since others might inherit from this one but are more specific.
            else if (actionExecutedContext.Exception is CoursesAPIException)
            {
                /*var error = new HttpError();
                error.Add("CoursesAPIException", actionExecutedContext.Exception.Message);

                actionExecutedContext.Response = actionExecutedContext.ActionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);*/

                /*actionExecutedContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(actionExecutedContext.Exception.Message),
                    StatusCode = HttpStatusCode.BadRequest
                };*/
            }
        }

        private HttpError GetErrors(ModelStateDictionary modelState, bool includeErrorDetail)
        {
            var modelStateError = new HttpError();
            foreach (KeyValuePair<string, ModelState> keyModelStatePair in modelState)
            {
                string key = keyModelStatePair.Key;
                ModelErrorCollection errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    IEnumerable<string> errorMessages = errors.Select(error =>
                    {
                        if (includeErrorDetail && error.Exception != null)
                        {
                            return error.Exception.Message;
                        }
                        return String.IsNullOrEmpty(error.ErrorMessage) ? "ErrorOccurred" : error.ErrorMessage;
                    }).ToArray();
                    modelStateError.Add(key, errorMessages);
                }
            }

            return modelStateError;
        }
    }
}
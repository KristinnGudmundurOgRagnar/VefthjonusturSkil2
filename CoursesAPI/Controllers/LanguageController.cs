using CoursesAPI.Filters;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Tracing;

namespace CoursesAPI.Controllers
{
    [RoutePrefix("api/language")]
    public class LanguageController : ApiController
    {

		private readonly CoursesServiceProvider _service;

        public LanguageController()
		{
			_service = new CoursesServiceProvider(new UnitOfWork<AppDataContext>());
		}

        [HttpGet]
        [Route("")]
        public IEnumerable<LanguageViewModel> Get()
        {
            //throw new Exception();

            Configuration.Services.GetTraceWriter().Info(Request, "Get", "Get the list of LanguageViewModel.");

            var languageViewModel = new LanguageViewModel
            {
                Description = Resources.Resources.Description,
                Timestamp = DateTime.UtcNow,
                Name = Resources.Resources.Name
            };
            return new[] { languageViewModel };
        }

        [HttpGet]
        [Route("{id}")]
        [AppExceptionFilter]
        public LanguageViewModel Get(int id)
        {
            LanguageViewModel model = _service.GetLanguageById(id);
            if (model == null)
            {
                //throw new HttpResponseException(HttpStatusCode.NotFound);

                HttpError error = new HttpError();
                error.Add("IdDoesNotExistException", Resources.Resources.IdDoesNotExistException + " Id: " + id);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound, error);

                throw new HttpResponseException(response);
            }
            return model;
        }

        [HttpGet]
        [Route("getexception")]
        public LanguageViewModel GetException()
        {
            string name = "";
            return _service.GetLanguageByName(name);
        }

        [HttpPost]
        [Route("")]
        public LanguageViewModel Post(LanguageViewModel model)
        //public HttpResponseMessage Post(LanguageViewModel model)
        {

            //Try to create instace of model
            return _service.CreateLanguage(model);

            //Old code, logic moved to service layer
            /*if (!ModelState.IsValid)
            {
                HttpError error = GetErrors(ModelState, true);
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);

            }

            return new HttpResponseMessage(HttpStatusCode.Created);*/
        }

        [HttpPost]
        [Route("{id}")]
        //public void Delete(int id)
        public HttpResponseMessage Delete(int id)
        {
            /*var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(Resources.Resources.IdDoesNotExistException + " Id: " + id),
            };*/

            var modelStateError = new HttpError();
            modelStateError.Add("IdDoesNotExistException", Resources.Resources.IdDoesNotExistException + " Id: " + id);

            //return Request.CreateResponse(HttpStatusCode.BadRequest, modelStateError);

            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, modelStateError));
        }


        // This method is required because the default BadRequest(Modelstate) adds a english message...
        private HttpError GetErrors(IEnumerable<KeyValuePair<string, ModelState>> modelState, bool includeErrorDetail)
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

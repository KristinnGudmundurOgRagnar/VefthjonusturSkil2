using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;

namespace CoursesAPI.Services.Exceptions
{
    public class CoursesAPIValidationException : CoursesAPIException
    {
        public List<ValidationResult> ValidationResults { get; set; }

        public CoursesAPIValidationException(List<ValidationResult> results)
			: base("ValidationException")
		{
			ValidationResults = results;
		}

		/// <summary>
		/// A constructor which accepts a single string key as a parameter.
		/// </summary>
		/// <param name="msgKey"></param>
        public CoursesAPIValidationException(string msgKey)
			: base(msgKey)
		{
			ValidationResults = new List<ValidationResult>
			{
				new ValidationResult(msgKey)
			};
		}

        /// <summary>
        /// This method will get called from within an exception handler,
        /// so we don't want any errors to propagate from it.
        /// </summary>
        /// <returns></returns>
        public string GetMessage()
        {
            //TODO: we do not want the error to be in a single string format, instead we want to normalize the validation errors to string dic format, key and value
            var b = new StringBuilder();
            try
            {
                foreach (var ex in ValidationResults)
                {
                    b.Append(ex.ToString());
                    b.Append(System.Environment.NewLine);
                }
                return b.ToString();
            }
            catch
            {
                return "UNSPECIFIED_ERROR";
            }
        }

        public List<ValidationResult> GetValidationResults()
        {
            return ValidationResults;
        }
    }
}

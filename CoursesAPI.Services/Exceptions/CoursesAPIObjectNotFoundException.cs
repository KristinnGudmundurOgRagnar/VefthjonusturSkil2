using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Services.Exceptions
{
    public class CoursesAPIObjectNotFoundException : ApplicationException
    {
        public List<ValidationResult> ValidationResults { get; set; }

        public CoursesAPIObjectNotFoundException(string message)
			: base(message)
        {
            ValidationResults = new List<ValidationResult>
			{
				new ValidationResult(message)
			};        
        }        

        public List<ValidationResult> GetValidationResults()
        {
            return ValidationResults;
        }
    }
}

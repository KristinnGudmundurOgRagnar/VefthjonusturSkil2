using CoursesAPI.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Services.Helpers
{
    /// <summary>
    /// A generic class which helps in validating models outside of 
    /// a Controller class. The main code comes from here:
    /// http://stackoverflow.com/questions/17047065/is-getting-modelstate-isvalid-functionality-outside-of-a-controller-possible
    /// 
    /// Usage:
    /// public void AddStuff(StuffViewModel model)
    /// {
    ///		CentrisValidation.Validate(model);
    ///		
    ///		// Continue, if validation fails then an error will be thrown.
    /// }
    /// It is assumed that the xxxViewModel class uses attribute validation.
    /// </summary>
    public class CourseAPIValidation
    {
        public static void Validate<T>(T model)
        {            
            var results = new List<ValidationResult>();
            if (model == null)
            {
                //Add custom error code which is used to retrieve the error message in the correct lang in the filter at the API level
                results.Add(new ValidationResult(ErrorCodes.IdDoesNotExistException));
                throw new CoursesAPIValidationException(results);
            }

            var context = new ValidationContext(model, null, null);

            if (!Validator.TryValidateObject(model, context, results))
            {                
                //No need to inject results in exception. 
                throw new CoursesAPIValidationException(results);
            }
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace CoursesAPI.Models
{
    /// <summary>
    /// Language ViewModel
    /// </summary>
    public class LanguageViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), AllowEmptyStrings = false, ErrorMessageResourceName = "NameRequired")]
        [StringLength(5, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "FirstNameLong")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), AllowEmptyStrings = false, ErrorMessageResourceName = "DescriptionRequired")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), AllowEmptyStrings = false, ErrorMessageResourceName = "TimestampRequired")]
        public DateTime Timestamp { get; set; }
    }
}
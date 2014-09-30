using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Services.Exceptions
{
    public class CoursesAPIException : ApplicationException
    {
        public CoursesAPIException(string msg)
			: base(msg){}
    }
}

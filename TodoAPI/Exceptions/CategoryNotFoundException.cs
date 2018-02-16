using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Exceptions
{
    public class CategoryNotFoundException : MvcException
    {
        public CategoryNotFoundException()
        {
        }

        public CategoryNotFoundException(int httpErrorCode,string message)
            : base(httpErrorCode,message)
        {
        }

        public CategoryNotFoundException(int httpErrorCode,string message, Exception inner)
            : base(httpErrorCode,message, inner)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Exceptions
{
    public class MvcException : Exception
    {
        public int HttpErrorCode { get; set; }
        public MvcException()
        {

        }
        public MvcException(int httpErrorCode, string message)
              : base(message)
        {
            HttpErrorCode = httpErrorCode;
        }
        public MvcException(int httpErrorCode, string message, Exception inner)
            : base(message, inner)
        {
            HttpErrorCode = httpErrorCode;
        }
    }
}

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Mvc.ApplicationModels;

using Microsoft.Extensions.Configuration;

namespace TodoAPI
{
    public class NameSpaceVersionRoutingcs : IApplicationModelConvention
    {
        private readonly string apiPrefix;

       //private const string urlTemplate = &quot; //= &quot;{0}/{1}/{2}&quot;;

   
       public void Apply(ApplicationModel application)
        {
            throw new NotImplementedException();
        }
    }
}

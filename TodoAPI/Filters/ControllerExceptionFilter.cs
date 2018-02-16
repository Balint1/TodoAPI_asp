using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Exceptions;

namespace TodoAPI.Filters
{
    public class ControllerExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public ControllerExceptionFilter(
            IHostingEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {
            if (!_hostingEnvironment.IsDevelopment())
            {
                // do nothing
                return;
            }
            if (context.Exception == null)
                return;
             MvcException me = (MvcException)context.Exception;
             switch (me.HttpErrorCode)
             {
                 case 404:
                     context.Result = new NotFoundObjectResult(context.Exception.Message);
                     break;
                 default:
                     context.Result = new BadRequestObjectResult(context.Exception.Message);
                     break;
             }
            //context.Result = new BadRequestObjectResult(context.Exception.Message);

        }
    }
}

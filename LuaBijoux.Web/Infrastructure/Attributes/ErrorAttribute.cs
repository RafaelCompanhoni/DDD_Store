using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LuaBijoux.Core.Logging;

namespace LuaBijoux.Web.Infrastructure.Attributes
{
    public class ErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            ILogger logger = (ILogger) DependencyResolver.Current.GetService(typeof (ILogger));
            logger.Log(filterContext.Exception);
        }
    }
}
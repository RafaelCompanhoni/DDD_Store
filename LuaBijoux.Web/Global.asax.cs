using System;
using System.Web.Mvc;
using System.Web.Routing;
using LuaBijoux.Core.Logging;
using LuaBijoux.Web.Infrastructure.Mappers;
using LuaBijoux.Web.Infrastructure.Utils;

namespace LuaBijoux.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AutoMapperConfig.RegisterMappings();
        }

        void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception == null)
                return;

            ILogger logger = (ILogger)DependencyResolver.Current.GetService(typeof(ILogger));
            logger.Log(exception);

            Server.ClearError();

            Response.Redirect("~/Admin/Errors/Error");
        }
    }
}

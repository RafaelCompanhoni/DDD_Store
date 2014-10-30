using System;
using System.Web.Mvc;
using System.Web.Routing;
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
            // NLog e redirect
        }
    }
}

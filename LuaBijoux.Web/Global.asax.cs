using System.Web.Mvc;
using System.Web.Routing;
using LuaBijoux.Web.Infrastructure.Mappers;

namespace LuaBijoux.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AutoMapperConfig.RegisterMappings();
        }
    }
}

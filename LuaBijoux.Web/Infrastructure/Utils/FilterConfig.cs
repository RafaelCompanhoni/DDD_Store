using System.Web.Mvc;
using LuaBijoux.Web.Infrastructure.Attributes;

namespace LuaBijoux.Web.Infrastructure.Utils
{
    public class FilterConfig
    {
        // registrar aqui os filtros globais - serão aplicados em todos os action methods de todos os controllers
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // Customização de HandleError - utiliza o logger para registrar as exceções
            filters.Add(new ErrorAttribute());
        }
    }
}
using System.Web.Mvc;
using LuaBijoux.Bootstrapper;
using LuaBijoux.Core.Data;
using LuaBijoux.Core.Logging;
using LuaBijoux.Core.Services;
using LuaBijoux.Data;
using LuaBijoux.Infrastructure.Logging;
using LuaBijoux.Services;
using LuaBijoux.Web;
using Autofac;
using Autofac.Integration.Mvc;
using LuaBijoux.Web.Areas.Admin.Controllers;

//Especifica-se que o método RegisterDependencies de IocConfig será chamado no ínicio do pipeline de inicialização da aplicação (antes de Application_Start de Global.asax)
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(IocConfig), "RegisterDependencies")]

namespace LuaBijoux.Bootstrapper
{
    public class IocConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterGeneric(typeof(EntityRepository<>)).As(typeof(IRepository<>));
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>));
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork));

            const string nameOrConnectionString = "name=AppContext";
            builder.Register<IEntitiesContext>(b =>
            {
                var context = new LuaBijouxContext(nameOrConnectionString);
                return context;
            });

            builder.RegisterType(typeof(NLogLogger)).As<ILogger>().SingleInstance();
            builder.RegisterModule(new IdentityModule());

            // Controllers - necessário para implementação via reflexão em RemoteClientServerAttribute
            builder.RegisterType<UsersController>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}

using System.Data.Entity;
using System.Web;
using LuaBijoux.Core.Identity;
using LuaBijoux.Data;
using LuaBijoux.Data.Identity;
using LuaBijoux.Data.Identity.Models;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LuaBijoux.Bootstrapper
{
    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(ApplicationUserManager)).As(typeof(IApplicationUserManager));
            builder.RegisterType(typeof(ApplicationRoleManager)).As(typeof(IApplicationRoleManager));
            builder.RegisterType(typeof(ApplicationIdentityUser)).As(typeof(IUser<int>));
            builder.Register(b => b.Resolve<IEntitiesContext>() as DbContext);
            builder.Register(b =>
            {
                var manager = IdentityFactory.CreateUserManager(b.Resolve<DbContext>());
                if (Startup.DataProtectionProvider != null)
                {
                    manager.UserTokenProvider =
                        new DataProtectorTokenProvider<ApplicationIdentityUser, int>(
                            Startup.DataProtectionProvider.Create("ASP.NET Identity"));
                }
                return manager;
            });
            builder.Register(b => IdentityFactory.CreateRoleManager(b.Resolve<DbContext>()));
            builder.Register(b => HttpContext.Current.GetOwinContext().Authentication);
        }
    }
}

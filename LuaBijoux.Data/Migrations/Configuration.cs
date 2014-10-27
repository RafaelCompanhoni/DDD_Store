using Microsoft.AspNet.Identity;
using LuaBijoux.Data.Identity;
using LuaBijoux.Data.Identity.Models;

namespace LuaBijoux.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<LuaBijouxContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "LuaBijoux.Data.LuaBijouxContext";
        }

        protected override void Seed(LuaBijouxContext context)
        {
            const string username = "admin@admin.com";
            const string password = "123456";
            const string roleName = "Administrator";

            var applicationRoleManager = IdentityFactory.CreateRoleManager(context);
            var applicationUserManager = IdentityFactory.CreateUserManager(context);


            //Create Role Admin if it does not exist
            var role = applicationRoleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationIdentityRole { Name = roleName };
                applicationRoleManager.Create(role);
            }

            var user = applicationUserManager.FindByName(username);
            if (user == null)
            {
                user = new ApplicationIdentityUser { UserName = username, Email = username };
                applicationUserManager.Create(user, password);
                applicationUserManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = applicationUserManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                applicationUserManager.AddToRole(user.Id, role.Name);
            }

            // var context = new LuaBijouxContext("name=AppContext", new DebugLogger());
            context.SaveChanges();
        }
    }
}

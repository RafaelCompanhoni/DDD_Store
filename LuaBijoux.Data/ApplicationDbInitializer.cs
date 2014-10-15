using System;
using System.Data.Entity;
using LuaBijoux.Core.DomainModels;
using LuaBijoux.Core.Logging;
using LuaBijoux.Data.Identity;
using LuaBijoux.Data.Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LuaBijoux.Data
{
    // Impede que o banco seja deletado quando existirem novas migrations
    public class ApplicationDbInitializer : NullDatabaseInitializer<LuaBijouxContext>
    {
    }

    /*
    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer : DropCreateDatabaseAlways<LuaBijouxContext>
    {
        protected override void Seed(LuaBijouxContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public void InitializeIdentityForEF(LuaBijouxContext db)
        {
            // This is only for testing purpose
            const string name = "admin@admin.com";
            const string password = "Admin@123456";
            const string roleName = "Admin";
            var applicationRoleManager = IdentityFactory.CreateRoleManager(db);
            var applicationUserManager = IdentityFactory.CreateUserManager(db);
            //Create Role Admin if it does not exist
            var role = applicationRoleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationIdentityRole{Name= roleName};
                applicationRoleManager.Create(role);
            }

            var user = applicationUserManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationIdentityUser { UserName = name, Email = name };
                applicationUserManager.Create(user, password);
                applicationUserManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = applicationUserManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                applicationUserManager.AddToRole(user.Id, role.Name);
            }
            var context = new LuaBijouxContext("name=AppContext",new DebugLogger());
            var image = new Image {Path = "http://lorempixel.com/400/200/"};
            context.Set<Image>().Add(image);
            for (var i = 0; i < 100; i++)
            {
                context.Set<Product>().Add(new Product { Name = "My Product", Description = "My Product", Image = image });
            }
            context.SaveChanges();
        }
        class DebugLogger : ILogger
        {
            public void Log(string message)
            {
                
            }

            public void Log(Exception ex)
            {
                
            }
        }
    }
    */
}
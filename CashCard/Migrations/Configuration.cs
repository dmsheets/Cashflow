using CashCard.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CashCard.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CashCard.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "CashCard.Models.ApplicationDbContext";
        }

        protected override void Seed(CashCard.Models.ApplicationDbContext context)
        {
            if (!context.Branches.Any(p => p.Name == "Pusat"))
            {
                var b = new Branch {Name = "Pusat", PengeluaranRegular = 1000};
                context.Branches.Add(b);
                context.SaveChanges();
            }

            //Role Management
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            if (!context.Roles.Any(p => p.Name == "User"))
            {

                var role = new IdentityRole {Name = "User"};
                roleManager.Create(role);
            }
            if (!context.Roles.Any(p => p.Name == "Supervisor"))
            {

                var role = new IdentityRole { Name = "Supervisor" };
                roleManager.Create(role);
            }
        




            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            if (!context.Users.Any(p => p.UserName == "Tanto"))
            {

                var user = new ApplicationUser {UserName = "Tanto",BranchId = 1};
                manager.Create(user, "123456");
               


            }

            var userx = manager.FindByName("Tanto");
            if (!manager.IsInRole(userx.Id, "User"))
            {
                manager.AddToRole(userx.Id, "User");
            }


              
            


        }
    }
}

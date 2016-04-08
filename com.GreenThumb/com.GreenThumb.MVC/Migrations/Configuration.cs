namespace com.GreenThumb.MVC.Migrations
{
    using com.GreenThumb.BusinessLogic;
    using com.GreenThumb.BusinessObjects;
    using com.GreenThumb.MVC.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security.Claims;

    internal sealed class Configuration : DbMigrationsConfiguration<com.GreenThumb.MVC.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(com.GreenThumb.MVC.Models.ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            const string userName = "admin";
            const string password = "P@ssword1";

            if (!context.Users.Any(u => u.UserName.Equals(userName)))
            {
                var user = new ApplicationUser()
                {
                    UserName
                        = userName,
                    Email
                        = "admin@admin.com",
                    FirstName
                        = "System",
                    LastName
                        = "Administrator"
                };



                IdentityResult result = userManager.Create(user, password);
                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Administrator" });
                context.SaveChanges();

                userManager.AddToRole(user.Id, "Administrator");
                var gt_userManager = new UserManager();

                int userId = 0;

                if (!gt_userManager.UserExists(userName, password))
                {
                    if (!gt_userManager.CreateNewUser(new User()
                    {
                        UserName
                            = user.UserName,
                        FirstName
                            = user.FirstName,
                        LastName
                            = user.LastName,
                        EmailAddress
                            = user.Email,
                        Zip
                            = "00000"
                    }, password))
                    {
                        throw new InvalidOperationException("User could not be created on the previous database.");
                    }
                }

                userId = gt_userManager.RetrieveUserId(userName);

                userManager.AddClaim(user.Id, new Claim("GTUserID", userId.ToString()));
                userManager.AddClaim(user.Id, new Claim(ClaimTypes.GivenName, user.FirstName));
                userManager.AddClaim(user.Id, new Claim(ClaimTypes.Surname, user.LastName));

                context.SaveChanges();
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}

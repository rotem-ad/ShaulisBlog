namespace ShaulisBlog.Migrations
{
    using ShaulisBlog.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<ShaulisBlog.DAL.BlogDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ShaulisBlog.DAL.BlogDBContext context)
        {
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

            // Added for SimpleMemberProvider (DO NOT REMOVE):
            WebSecurity.InitializeDatabaseConnection("BlogDBContext", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            SeedMembership();
            var fans = new List<Fan>
            {
                new Fan { FanID = 1, FName = "Eric", LName = "Cartman", Gender = "Male", BDate = DateTime.Parse("2000-09-01"), Seniority = 3 },
                new Fan { FanID = 2, FName = "Stan", LName = "Marsh", Gender = "Male", BDate = DateTime.Parse("1999-12-03"), Seniority = 6 },
                new Fan { FanID = 3, FName = "Mis", LName = "Piggy", Gender = "Female", BDate = DateTime.Parse("2002-08-10"), Seniority = 1 },
            };

            // Add above fans in case their FanID does not exist in DB, else just perform an update
            fans.ForEach(f => context.Fans.AddOrUpdate(p => p.FanID, f));
            context.SaveChanges();

            var locations = new List<Location>
            {
                new Location { LocationId = 1, Name = "Tel Aviv", Latitude = 32.074504, Longitude = 34.792191, ZIndex = 1, Description = "I like shopping at Azrieli mall!" },
                new Location { LocationId = 2, Name = "Herzliya", Latitude = 32.163767, Longitude = 34.797646, ZIndex = 1, Description = "Took Irena to Herzliya marine" },
                new Location { LocationId = 3, Name = "Eilat", Latitude = 29.557654, Longitude = 34.951926, ZIndex = 1, Description = "Hangining out in Eilat with my son Luther" },
                new Location { LocationId = 4, Name = "Safari", Latitude = 32.043882, Longitude = 34.825139, ZIndex = 1, Description = "At Safari with my favorite veterinarian - George!" },
                new Location { LocationId = 5, Name = "Ashdod", Latitude = 31.809848, Longitude = 34.655708, ZIndex = 1, Description = "Having breakfast with all parlament members (:" },
            };

            // Add above fans in case their FanID does not exist in DB, else just perform an update
            locations.ForEach(x => context.Locations.AddOrUpdate(p => p.LocationId, x));
            context.SaveChanges();
        }

        private void SeedMembership() // Inserts admin user + Administrator user group(role)
        {
            
            if (!Roles.RoleExists("Administrator"))
                Roles.CreateRole("Administrator");

            if (!WebSecurity.UserExists("zeev"))
                WebSecurity.CreateUserAndAccount(
                    "zeev",
                    "password"
                    //new { Mobile = "+19725000000", IsSmsVerified = false });
                );

            if (!Roles.GetRolesForUser("zeev").Contains("Administrator"))
                Roles.AddUsersToRoles(new[] { "zeev" }, new[] { "Administrator" });
        }
    }

}

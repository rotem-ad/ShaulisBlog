namespace ShaulisBlog.Migrations
{
    using ShaulisBlog.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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

            var fans = new List<Fan>
            {
                new Fan { FanID = 1, FName = "Eric", LName = "Cartman", Gender = "Male", BDate = DateTime.Parse("2000-09-01"), Seniority = 3 },
                new Fan { FanID = 2, FName = "Stan", LName = "Marsh", Gender = "Male", BDate = DateTime.Parse("1999-12-03"), Seniority = 6 },
                new Fan { FanID = 3, FName = "Mis", LName = "Piggy", Gender = "Female", BDate = DateTime.Parse("2002-08-10"), Seniority = 1 },
            };

            // Add above fans in case their FanID does not exist in DB, else just perform an update
            fans.ForEach(f => context.Fans.AddOrUpdate(p => p.FanID, f));
            context.SaveChanges();
        }
    }
}

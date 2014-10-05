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

            var posts = new List<Post>
            {
                new Post { PostID = 1, Author = "Terrance", AuthorWebSite = "http://tap.com", Title = "South Park", PublishDate = DateTime.Parse("2012-03-20"), Content = "Terrance and Philip rules!! Hello all SP Fans!!!" },
                new Post { PostID = 2, Author = "Timmy", AuthorWebSite = "http://timtimmy.sp.com", Title = "Timmy's Post", PublishDate = DateTime.Parse("2002-12-12"), Content = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut." },
                new Post { PostID = 3, Author = "Secret", AuthorWebSite = "http://mysecret.com", Title = "Special Post", PublishDate = DateTime.Parse("2013-10-12"), Content = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut la. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut la" },
                new Post { PostID = 4, Author = "Shauli", AuthorWebSite = "http://shauli.com", Title = "To Irena", PublishDate = DateTime.Parse("2014-10-04"), Content = "I love my beautiful Irena (:" }
            };

            // Add above posts in case their PostID does not exist in DB, else just perform an update
            posts.ForEach(p => context.Posts.AddOrUpdate(x => x.PostID, p));
            context.SaveChanges();

            var comments = new List<Comment>
            {
                new Comment { CommentID = 1, PostID = 1, Writer = "Eric Cartman", WriterWebSite = "http://eric-cart.com", Title = "Important", Content = "Screw you guys Im going home" },
                new Comment { CommentID = 2, PostID = 1, Writer = "James Bond", WriterWebSite = null, Title = "Cool Comment", Content = "Hello cool world" },
                new Comment { CommentID = 3, PostID = 1, Writer = "Barak Obama", WriterWebSite = "http://barak007.com", Title = "Barak Title", Content = "Peace on earth!! Hello all (:" },
                new Comment { CommentID = 4, PostID = 1, Writer = "Stan Marsh", WriterWebSite = "http://stan-darsh.com", Title = "OMG", Content = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut la. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut la" },
                new Comment { CommentID = 5, PostID = 3, Writer = "Jimmy", WriterWebSite = null, Title = "Jimmy's Title", Content = "What is your secret?" },
                new Comment { CommentID = 6, PostID = 3, Writer = "Donna", WriterWebSite = "http://helloworld.com", Title = "Hmmm", Content = "Why so serious?" },
                new Comment { CommentID = 7, PostID = 4, Writer = "Irena", WriterWebSite = null, Title = "Happy", Content = "I love you too Shauli!" },
            };

            // Add above comments in case their PostID does not exist in DB, else just perform an update
            comments.ForEach(c => context.Comments.AddOrUpdate(x => x.CommentID, c));
            context.SaveChanges();
            
            var fans = new List<Fan>
            {
                new Fan { FanID = 1, FName = "Eric", LName = "Cartman", Gender = "Male", BDate = DateTime.Parse("2000-09-01"), Seniority = 3 },
                new Fan { FanID = 2, FName = "Stan", LName = "Marsh", Gender = "Male", BDate = DateTime.Parse("1999-12-03"), Seniority = 6 },
                new Fan { FanID = 3, FName = "Mis", LName = "Piggy", Gender = "Female", BDate = DateTime.Parse("2002-08-10"), Seniority = 1 },
                new Fan { FanID = 3, FName = "Lady", LName = "Gaga", Gender = "Female", BDate = DateTime.Parse("2004-10-10"), Seniority = 3 }
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
                new Location { LocationId = 5, Name = "Ashdod", Latitude = 31.809848, Longitude = 34.655708, ZIndex = 1, Description = "Having breakfast with all parlament members (:" }
            };

            // Add above locations in case their LocationId does not exist in DB, else just perform an update
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

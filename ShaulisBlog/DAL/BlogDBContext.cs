using ShaulisBlog.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ShaulisBlog.DAL
{
    public class BlogDBContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        // Defines set of fan objects
        public DbSet<Fan> Fans { get; set; }
        public DbSet<Location> Locations { get; set; }

        // Login & Authentication management system:
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UsersInRole> UsersInRoles { get; set; }


        /*
         * The modelBuilder.Conventions.Remove statement in the OnModelCreating method prevents table names from being pluralized.
         * If you didn't do this, the generated tables would be named Posts and Comments.
         */
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }


    //    public class UsersContext : DbContext
    //{
    //    public UsersContext()
    //        : base("DefaultConnection")
    //    {
    //    }

        
    }
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMSClient.Model;
using System.Reflection.Emit;

namespace SMSClient.Data.Identity
{
    public class AspIdUsersContext: IdentityDbContext<ApplicationUser>
    {
       public AspIdUsersContext(DbContextOptions<AspIdUsersContext> options)
    : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Additional configurations as needed...

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}

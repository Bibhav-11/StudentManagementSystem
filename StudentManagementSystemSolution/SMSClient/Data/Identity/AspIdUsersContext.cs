using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMSClient.Models;
using SMSClient.Models.Identity;
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
        public DbSet<Course> Courses { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<DepartmentSemester> DepartmentSemesters { get; set; }
        public DbSet<SemesterCourse> SemesterCourses { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Student> Students { get; set; }

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

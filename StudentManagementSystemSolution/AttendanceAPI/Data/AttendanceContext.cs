using AttendanceAPI.DateOnlyConverters;
using AttendanceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAPI.Data
{
    public class AttendanceContext: DbContext
    {
        public AttendanceContext(DbContextOptions<AttendanceContext> options): base(options)
        {
            
        }

        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            base.ConfigureConventions(builder);
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>();
        }
    }
}

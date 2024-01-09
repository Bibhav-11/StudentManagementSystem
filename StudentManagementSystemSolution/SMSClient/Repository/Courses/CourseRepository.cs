using Microsoft.EntityFrameworkCore;
using SMSClient.Data.Identity;
using SMSClient.Model;

namespace SMSClient.Repository.Courses
{
    public class CourseRepository: Repository<Course>, ICourseRepositiory
    {
        private readonly AspIdUsersContext _context;

        public CourseRepository(AspIdUsersContext context): base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetCourseWithClassInfo()
        {
            return await _context.Courses.Include(c => c.Class).ToListAsync();
        }
    }
}

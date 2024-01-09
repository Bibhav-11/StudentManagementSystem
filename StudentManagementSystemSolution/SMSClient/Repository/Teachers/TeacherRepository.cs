using Microsoft.EntityFrameworkCore;
using SMSClient.Data.Identity;
using SMSClient.Model;

namespace SMSClient.Repository.Teachers
{
    public class TeacherRepository: Repository<Teacher>, ITeacherRepository
    {
        private readonly AspIdUsersContext _context;

        public TeacherRepository(AspIdUsersContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Teacher>> GetTeachersWithClassInfo()
        {
            return await _context.Teachers.Include(c => c.Class).ToListAsync();
        }
    }
}

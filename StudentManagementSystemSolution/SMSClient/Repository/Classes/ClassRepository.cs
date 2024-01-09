using Microsoft.EntityFrameworkCore;
using SMSClient.Data.Identity;
using SMSClient.Model;

namespace SMSClient.Repository.Classes
{
    public class ClassRepository: Repository<Class>, IClassRepository
    {
        private readonly AspIdUsersContext _context;
        public ClassRepository(AspIdUsersContext context): base(context) 
        {
            _context = context;
        }

        public async Task<Class?> GetClassWithRelatedEntities(int classId)
        {
            return await _context.Classes.Include(c => c.Courses).Include(c => c.Students).Include(c => c.Teachers).SingleOrDefaultAsync(c => c.Id == classId);
        }

        public async Task<IEnumerable<Class>> GetClassWithDepartmentInfo()
        {
            return await _context.Classes.Include(c => c.Department).ToListAsync();
        }
    }
}

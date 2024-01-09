using Microsoft.EntityFrameworkCore;
using SMSClient.Data.Identity;
using SMSClient.Model;

namespace SMSClient.Repository.Departments
{
    public class DepartmentRepository: Repository<Department>, IDepartmentRepository
    {
        private readonly AspIdUsersContext _context;

        public DepartmentRepository(AspIdUsersContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Department?> GetDepartmentWithClassInfo(int departmentId)
        {
            return await _context.Departments.Include(d => d.Classes).SingleOrDefaultAsync(d => d.Id == departmentId);
        }
    }
}

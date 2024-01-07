using Microsoft.EntityFrameworkCore;
using SMSClient.Data.Identity;
using SMSClient.Models;

namespace SMSClient.Repository.Students
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly AspIdUsersContext _context;

        public StudentRepository(AspIdUsersContext context): base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetStudentsWithDepartmentAndSemesterInfo()
        {
            return await _context.Students.Include(s => s.Department).Include(s => s.Semester).ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsWithUserAndUserInfo()
        {
            return await _context.Students.Include(s => s.ApplicationUser).ToListAsync();
        }


    }
}

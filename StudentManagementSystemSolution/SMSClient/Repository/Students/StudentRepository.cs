using Microsoft.EntityFrameworkCore;
using SMSClient.Data.Identity;
using SMSClient.Model;

namespace SMSClient.Repository.Students
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly AspIdUsersContext _context;

        public StudentRepository(AspIdUsersContext context): base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetByClassId(int classId)
        {
            return await _context.Students.Where(s => s.ClassId == classId).ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsWithClassInfo()
        {
            return await _context.Students.Include(s => s.Class).ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsWithDepartmentAndSemesterInfo()
        {
            return new List<Student>();
            //return await _context.Students.Include(s => s.Department).Include(s => s.Semester).ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsWithUserAndUserInfo()
        {
            return new List<Student>();
            //return await _context.Students.Include(s => s.ApplicationUser).ToListAsync();
        }


    }
}


using SMSClient.Data.Identity;
using SMSClient.Model;
using SMSClient.Repository;
using SMSClient.Repository.Classes;

namespace SMSClient.Service.Classes
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly AspIdUsersContext _context;

        public ClassService(IClassRepository classRepository, AspIdUsersContext context)
        {
            _classRepository = classRepository;
            _context = context;
        }

        public async Task AddClass(Class classForm)
        {
            _classRepository.Add(classForm);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClass(Class classForm)
        {
            foreach(var course in classForm.Courses)
            {
                course.ClassId = null;
            }
            foreach(var student in classForm.Students)
            {
                student.ClassId = null;
            }
            foreach(var teacher in  classForm.Teachers)
            {
                teacher.ClassId = null;
            }
            _classRepository.Remove(classForm);
            await _context.SaveChangesAsync();
        }

        public Task<Class?> GetClassById(int id)
        {
            return _classRepository.GetById(id);
        }

        public Task<IEnumerable<Class>> GetClasses()
        {
            return _classRepository.GetAll();
        }

        public async Task<IEnumerable<Class>> GetClassWithDepartmentInfo()
        {
            return await _classRepository.GetClassWithDepartmentInfo();
        }

        public async Task<Class?> GetClassWithRelatedEntities(int classId)
        {
            return await _classRepository.GetClassWithRelatedEntities(classId);
        }

        public async Task UpdateClass(Class classForm)
        {
            _classRepository.Update(classForm);
            await _context.SaveChangesAsync();
        }
    }
}

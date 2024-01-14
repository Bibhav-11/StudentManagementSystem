using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SMSClient.Data.Identity;
using SMSClient.Repository;
using SMSClient.Repository.Students;
using SMSClient.Model;
using System.Security.Claims;
using SMSClient.Service.Classes;

namespace SMSClient.Service.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IClassService _classService;
        private readonly AspIdUsersContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public StudentService(IStudentRepository studentRepository, IRepository<UserInfo> userInfoRepository, AspIdUsersContext context, UserManager<ApplicationUser> userManager, IClassService classService)
        {
            _studentRepository = studentRepository;
            _userInfoRepository = userInfoRepository;
            _context = context;
            _userManager = userManager;
            _classService = classService;
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _studentRepository.GetAll();
        }

        public async Task<IEnumerable<Student>> GetStudentsByClassId(int classId)
        {
            return await _studentRepository.GetByClassId(classId);
        }

        public Task<Student?> GetStudentById(int id)
        {
            return _studentRepository.GetById(id);
        }

        public async Task<IEnumerable<Student>> GetStudentsWithDepartmentAndSemesterInfo()
        {
            var students = await _studentRepository.GetStudentsWithDepartmentAndSemesterInfo();
            return students;
        }

        public async Task<IEnumerable<Student>> GetStudentsUnAssignedToAnyDepartment()
        {
            return await _studentRepository.FindAll(s => s.ClassId == null);
        }

        public async Task<IEnumerable<Student>> GetStudentsUnAssignedToAnySemester()
        {
            var students = await _studentRepository.FindAll(s => s.ClassId == null);
            return students;
        }

        public async Task<bool> AddStudent(Student student)
        {
            var user = new ApplicationUser
            {
                Email = student.Email,
                UserName = student.Email,
            };

            var result = await _userManager.CreateAsync(user, "Pass123$");
            await _userManager.AddToRoleAsync(user, "student");
            if (result.Succeeded)
            {
                var createdUser = await _userManager.FindByNameAsync(student.Email);
                student.ApplicationUserId = createdUser.Id;
                _studentRepository.Add(student);
                await _context.SaveChangesAsync();
                var createdStudent = (await _studentRepository.FindAll(t => t.Email == student.Email)).SingleOrDefault();
                if (createdStudent != null)
                {
                    var studentClaim = new Claim("student", createdStudent.Id.ToString());
                    await _userManager.AddClaimAsync(createdUser, studentClaim);
                    return true;
                }
                return false;
            }
            return false;
        }


        public async Task<bool> UpdateStudent(Student student)
        {
            _studentRepository.Update(student);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        public async Task<bool> DeleteStudent(Student student)
        {
            if(student  == null) return false;
            var user = await _userManager.FindByIdAsync(student.ApplicationUserId);
            _studentRepository.Remove(student);
            await _context.SaveChangesAsync();
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<Student?> GetStudentByUserId(string userId)
        {
            return (await _studentRepository.FindAll(s => s.ApplicationUserId == userId)).FirstOrDefault();
        }

        public async Task<IEnumerable<Student>> GetStudentsWithClassInfo()
        {
            return await _studentRepository.GetStudentsWithClassInfo();
        }

        public async Task<IEnumerable<Student>> GetStudentsOfActiveClasses()
        {
            var activeClassIds = (await _classService.GetClassesOfActiveDepartment()).Select(c => c.Id).ToList();
            var activeStudents = await _studentRepository.FindAll(s => activeClassIds.Contains(s.ClassId.Value) || s.ClassId == null);
            return activeStudents;
        }
    }
}

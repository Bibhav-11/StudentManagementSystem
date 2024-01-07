using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SMSClient.Data.Identity;
using SMSClient.Models;
using SMSClient.Models.Identity;
using SMSClient.Models.ViewModel;
using SMSClient.Repository;
using SMSClient.Repository.Students;

namespace SMSClient.Service.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IRepository<DepartmentSemester> _departmentSemsterRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly AspIdUsersContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public StudentService(IStudentRepository studentRepository, IRepository<DepartmentSemester> departmentSemsterRepository, IRepository<UserInfo> userInfoRepository, AspIdUsersContext context, UserManager<ApplicationUser> userManager)
        {
            _studentRepository = studentRepository;
            _departmentSemsterRepository = departmentSemsterRepository;
            _userInfoRepository = userInfoRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _studentRepository.GetAll();
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
            return await _studentRepository.FindAll(s => s.DepartmentId == null);
        }

        public async Task<IEnumerable<Student>> GetStudentsUnAssignedToAnySemester()
        {
            var students = await _studentRepository.FindAll(s => s.SemesterId == null);
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
                var saved = await _context.SaveChangesAsync();
                return saved > 0;
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
            _studentRepository.Remove(student);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }









      
    }
}

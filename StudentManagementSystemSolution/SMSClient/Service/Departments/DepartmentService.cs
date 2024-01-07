using Microsoft.EntityFrameworkCore;
using SMSClient.Data.Identity;
using SMSClient.Models;
using SMSClient.Models.ViewModel;
using SMSClient.Repository;

namespace SMSClient.Service.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Semester> _semesterRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<DepartmentSemester> _departmentSemesterRepository;


        private readonly AspIdUsersContext _context;

        public DepartmentService(IRepository<Department> departmentRepository, IRepository<Semester> semesterRepository, IRepository<Course> courseRepository, AspIdUsersContext context, IRepository<Student> studentRepository, IRepository<DepartmentSemester> departmentSemesterRepository)
        {
            _departmentRepository = departmentRepository;
            _semesterRepository = semesterRepository;
            _courseRepository = courseRepository;
            _context = context;
            _studentRepository = studentRepository;
            _departmentSemesterRepository = departmentSemesterRepository;
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await _departmentRepository.GetAll();
        }

        public async Task<Department?> GetDepartmentById(int id)
        {
            return await _departmentRepository.GetById(id);
        }


        public async Task CreateDepartment(Department department)
        {
            _departmentRepository.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddDepartment(DepartmentViewModel departmentForm)
        {
            var department = new Department
            {
                ShortName = departmentForm.ShortName,
                LongName = departmentForm.LongName,
                Email = departmentForm.Email,
            };

            if (departmentForm.CourseCodes != null)
            {
                var courses = await _courseRepository.FindAll(c => departmentForm.CourseCodes.Contains(c.CourseCode));
                department.Courses.AddRange(courses);
            }

            if (departmentForm.SemesterIds != null)
            {
                var semesters = await _semesterRepository.FindAll(s => departmentForm.SemesterIds.Contains(s.Id));
                department.Semesters.AddRange(semesters);
            }

            if (departmentForm.StudentIds != null)
            {
                var students = await _studentRepository.FindAll(s => departmentForm.StudentIds.Contains(s.Id));
                department.Students.AddRange(students);
            }

            _departmentRepository.Add(department);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        public async Task<bool> UpdateDepartment(Department department)
        {
            _departmentRepository.Update(department);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        public async Task<bool> DeleteDepartment(Department department)
        {
            var students = await _studentRepository.FindAll(s => s.DepartmentId == department.Id);
            var departmentSemesters = await _departmentSemesterRepository.FindAll(ds => ds.DepartmentId == department.Id);
            var semesterIds = departmentSemesters.Select(ds => ds.SemesterId);
            var semesters = await _semesterRepository.FindAll(s => semesterIds.Contains(s.Id));
            var courses = await _courseRepository.FindAll(c => c.DepartmentId == department.Id);
            
            foreach(var student in students) department.Students.Remove(student);
            foreach(var semester in semesters) department.Semesters.Remove(semester);
            foreach(var course in courses) department.Courses.Remove(course);

            _departmentRepository.Remove(department);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

    }
}

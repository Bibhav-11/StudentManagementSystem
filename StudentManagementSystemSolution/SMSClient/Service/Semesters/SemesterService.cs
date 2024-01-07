using Microsoft.EntityFrameworkCore;
using SMSClient.Data.Identity;
using SMSClient.Models;
using SMSClient.Models.ViewModel;
using SMSClient.Repository;
using SMSClient.Repository.Students;

namespace SMSClient.Service.Semesters
{
    public class SemesterService : ISemesterService
    {
        private readonly IRepository<Semester> _semesterRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IStudentRepository _studentRepository;

        private readonly AspIdUsersContext _context;

        public SemesterService(IRepository<Semester> semesterRepository, IRepository<Course> courseRepository, IRepository<Department> departmentRepository, IStudentRepository studentRepository, AspIdUsersContext context)
        {
            _semesterRepository = semesterRepository;
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
            _studentRepository = studentRepository;
            _context = context;
        }

        public Task<IEnumerable<Semester>> GetSemesters()
        {
            return _semesterRepository.GetAll();
        }

        public Task<Semester?> GetSemesterById(int id)
        {
            return _semesterRepository.GetById(id);
        }


        public async Task<bool> AddSemester(SemesterViewModel semesterForm)
        {
            var semester = new Semester
            {
                ShortName = semesterForm.ShortName,
                LongName = semesterForm.LongName,
                StartDate = semesterForm.StartDate,
                EndDate = semesterForm.EndDate,
            };

            if (semesterForm.CourseCodes != null)
            {
                var courses = await _courseRepository.FindAll(c => semesterForm.CourseCodes.Contains(c.CourseCode));
                semester.Courses.AddRange(courses);
            }

            if (semesterForm.DepartmentIds != null)
            {
                var departments = await _departmentRepository.FindAll(d => semesterForm.DepartmentIds.Contains(d.Id));
                semester.Departments.AddRange(departments);
            }

            if (semesterForm.StudentIds != null)
            {
                var students = await _studentRepository.FindAll(s => semesterForm.StudentIds.Contains(s.Id));
                semester.Students.AddRange(students);
            }

            _semesterRepository.Add(semester);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        public async Task<bool> UpdateSemester(Semester semester)
        {
            _semesterRepository.Update(semester);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        public async Task<bool> DeleteSemester(Semester semester)
        {
            var students = await _studentRepository.FindAll(s => s.SemesterId == semester.Id);
            foreach(var student in students) { semester.Students.Remove(student); }
            _semesterRepository.Remove(semester);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

    }
}

using Microsoft.EntityFrameworkCore;
using SMSClient.Data.Identity;
using SMSClient.Models;
using SMSClient.Repository;

namespace SMSClient.Service.Courses
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly AspIdUsersContext _context;

        public CourseService(IRepository<Course> courseRepository, AspIdUsersContext context)
        {
            _courseRepository = courseRepository;
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await _courseRepository.GetAll();
        }

        public async Task<Course?> GetCourseById(string id)
        {
            return await _courseRepository.GetById(id);
        }

        public Task<IEnumerable<Course>> GetUnassignedCoursesofDepartment()
        {
            var courses = _courseRepository.FindAll(c => c.DepartmentId == null);
            return courses;
        }


        public async Task CreateCourse(Course course)
        {
            _courseRepository.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCourse(Course course)
        {
            _courseRepository.Update(course);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteCourse(Course course)
        {
            _courseRepository.Remove(course);
            await _context.SaveChangesAsync();
        }

    }
}

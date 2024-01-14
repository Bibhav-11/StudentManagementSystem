using Microsoft.EntityFrameworkCore;
using SMSClient.Data.Identity;
using SMSClient.Model;
using SMSClient.Repository;
using SMSClient.Repository.Courses;
using SMSClient.Service.Classes;

namespace SMSClient.Service.Courses
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepositiory _courseRepository;
        private readonly IClassService _classService;
        private readonly AspIdUsersContext _context;

        public CourseService(ICourseRepositiory courseRepository, AspIdUsersContext context, IClassService classService)
        {
            _courseRepository = courseRepository;
            _context = context;
            _classService = classService;
        }

        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await _courseRepository.GetAll();
        }

        public async Task<IEnumerable<Course>> GetCoursesWithClassInfo()
        {
            return await _courseRepository.GetCourseWithClassInfo();
        }

        public async Task<Course?> GetCourseById(string id)
        {
            return await _courseRepository.GetById(id);
        }

        public IEnumerable<Course> GetUnassignedCoursesofDepartment()
        {
            //var courses = _courseRepository.FindAll(c => c.DepartmentId == null);
            return new List<Course>();
            //return courses;
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

        public async Task<IEnumerable<Course>> GetCoursesOfActiveClasses()
        {
            var activeClassesIds = (await _classService.GetClassesOfActiveDepartment()).Select(c => c.Id);
            var activeCourses = (await _courseRepository.FindAll(c => activeClassesIds.Contains(c.ClassId.Value) || c.ClassId == null)).ToList();
            return activeCourses;
        }
    }
}

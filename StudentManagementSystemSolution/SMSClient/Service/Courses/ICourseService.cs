using SMSClient.Models;
using SMSClient.Repository;

namespace SMSClient.Service.Courses
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetCourses();
        Task<Course?> GetCourseById(string id);
        Task<IEnumerable<Course>> GetUnassignedCoursesofDepartment();

        Task CreateCourse(Course course);

        Task UpdateCourse(Course course);

        Task DeleteCourse(Course course);
    }
}

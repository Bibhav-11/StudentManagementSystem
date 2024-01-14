using SMSClient.Model;
using SMSClient.Repository;

namespace SMSClient.Service.Courses
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetCourses();
        Task<IEnumerable<Course>> GetCoursesOfActiveClasses();
        Task<IEnumerable<Course>> GetCoursesWithClassInfo();
        Task<Course?> GetCourseById(string id);
        IEnumerable<Course> GetUnassignedCoursesofDepartment();

        Task CreateCourse(Course course);

        Task UpdateCourse(Course course);

        Task DeleteCourse(Course course);
    }
}

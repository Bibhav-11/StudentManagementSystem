using SMSClient.Model;

namespace SMSClient.Repository.Courses
{
    public interface ICourseRepositiory: IRepository<Course>
    {
        Task<IEnumerable<Course>> GetCourseWithClassInfo();
    }
}

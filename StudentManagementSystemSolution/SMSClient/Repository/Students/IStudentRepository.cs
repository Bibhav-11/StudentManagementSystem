using SMSClient.Models;

namespace SMSClient.Repository.Students
{
    public interface IStudentRepository: IRepository<Student>
    {
        Task<IEnumerable<Student>> GetStudentsWithUserAndUserInfo();
        Task<IEnumerable<Student>> GetStudentsWithDepartmentAndSemesterInfo();
    }
}

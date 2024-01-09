using SMSClient.Model;

namespace SMSClient.Repository.Students
{
    public interface IStudentRepository: IRepository<Student>
    {
        Task<IEnumerable<Student>> GetStudentsWithUserAndUserInfo();

        Task<IEnumerable<Student>> GetStudentsWithClassInfo();
        Task<IEnumerable<Student>> GetStudentsWithDepartmentAndSemesterInfo();
    }
}

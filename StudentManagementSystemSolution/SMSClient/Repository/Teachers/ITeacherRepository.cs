using SMSClient.Model;

namespace SMSClient.Repository.Teachers
{
    public interface ITeacherRepository: IRepository<Teacher>
    {
        Task<IEnumerable<Teacher>> GetTeachersWithClassInfo();
    }
}

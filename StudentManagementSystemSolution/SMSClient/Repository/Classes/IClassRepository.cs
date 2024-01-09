using SMSClient.Model;

namespace SMSClient.Repository.Classes
{
    public interface IClassRepository: IRepository<Class>
    {
        Task<Class?> GetClassWithRelatedEntities(int classId);
        Task<IEnumerable<Class>> GetClassWithDepartmentInfo();
    }
}

using SMSClient.Model;

namespace SMSClient.Service.Classes

{
    public interface IClassService
    {
        Task<IEnumerable<Class>> GetClasses();
        Task<Class?> GetClassById(int id);
        Task<Class?> GetClassWithRelatedEntities(int classId);
        Task<IEnumerable<Class>> GetClassWithDepartmentInfo();

        Task AddClass(Class classForm);

        Task UpdateClass(Class classForm);

        Task DeleteClass(Class classForm);
    }
}

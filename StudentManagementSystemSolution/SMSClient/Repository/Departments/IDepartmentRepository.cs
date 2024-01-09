using SMSClient.Model;

namespace SMSClient.Repository.Departments
{
    public interface IDepartmentRepository: IRepository<Department>
    {
        Task<Department?> GetDepartmentWithClassInfo(int departmentId);
    }

}

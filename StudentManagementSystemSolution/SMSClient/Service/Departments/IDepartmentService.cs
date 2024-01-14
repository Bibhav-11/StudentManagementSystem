using SMSClient.Model;

namespace SMSClient.Service.Departments
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetDepartments();
        Task<IEnumerable<Department>> GetActiveDepartments();
        Task<Department?> GetDepartmentById(int id);
        Task<Department?> GetDepartmentWithClassInfo(int departmentId);

        Task AddDepartment(Department department);
        Task CreateDepartment(Department department);

        Task UpdateDepartment(Department department);

        Task DeleteDepartment(Department department);

    }
}

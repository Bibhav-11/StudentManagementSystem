using SMSClient.Models;
using SMSClient.Models.ViewModel;

namespace SMSClient.Service.Departments
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetDepartments();
        Task<Department?> GetDepartmentById(int id);

        Task<bool> AddDepartment(DepartmentViewModel departmentForm);
        Task CreateDepartment(Department department);

        Task<bool> UpdateDepartment(Department department);

        Task<bool> DeleteDepartment(Department department);

    }
}

using Microsoft.EntityFrameworkCore;
using SMSClient.Data.Identity;
using SMSClient.Model;
using SMSClient.Repository;
using SMSClient.Repository.Departments;

namespace SMSClient.Service.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IRepository<Class> _classRepository;


        private readonly AspIdUsersContext _context;

        public DepartmentService(IDepartmentRepository departmentRepository, AspIdUsersContext context, IRepository<Class> classRepository)
        {
            _departmentRepository = departmentRepository;
            _context = context;
            _classRepository = classRepository;
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await _departmentRepository.GetAll();
        }

        public async Task<Department?> GetDepartmentById(int id)
        {
            return await _departmentRepository.GetById(id);
        }

        public async Task CreateDepartment(Department department)
        {
            _departmentRepository.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task AddDepartment(Department department)
        {
            _departmentRepository.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDepartment(Department department)
        {
            _departmentRepository.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartment(Department department)
        {
            foreach(var classEntity in department.Classes)
            {
                classEntity.DepartmentId = null;
            }
           _departmentRepository.Remove(department);
            await _context.SaveChangesAsync();
        }

        public async Task<Department?> GetDepartmentWithClassInfo(int departmentId)
        {
            return await _departmentRepository.GetDepartmentWithClassInfo(departmentId);
        }
    }
}

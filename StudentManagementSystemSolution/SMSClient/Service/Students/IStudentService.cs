using SMSClient.Model;

namespace SMSClient.Service.Students
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetStudents();
        Task<IEnumerable<Student>> GetStudentsWithClassInfo();

        Task<Student?> GetStudentById(int id);
        Task<Student?> GetStudentByUserId(string userId);
        Task<IEnumerable<Student>> GetStudentsWithDepartmentAndSemesterInfo();
        Task<IEnumerable<Student>> GetStudentsUnAssignedToAnyDepartment();
        Task<IEnumerable<Student>> GetStudentsUnAssignedToAnySemester();

        Task<bool> AddStudent(Student student);

        Task<bool> UpdateStudent(Student student);

        Task<bool> DeleteStudent(Student student);
    }
}

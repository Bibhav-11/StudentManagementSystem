using SMSClient.Model;

namespace SMSClient.Service.Teahcers
{
    public interface ITeacherService
    {

        Task<IEnumerable<Teacher>> GetTeachers();
        Task<IEnumerable<Teacher>> GetTeachersWithClassInfo();
        Task<Teacher?> GetTeacherById(int id);
        Task<Teacher?> GetTeacherByUserId(string userId);
        Task AddTeacher(Teacher teacher);

        Task UpdateTeacher(Teacher teacher);

        Task DeleteTeacher(Teacher teacher);
    }
}

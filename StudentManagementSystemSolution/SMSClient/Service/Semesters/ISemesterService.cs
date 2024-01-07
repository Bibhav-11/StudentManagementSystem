using SMSClient.Models;
using SMSClient.Models.ViewModel;

namespace SMSClient.Service.Semesters
{
    public interface ISemesterService
    {
        Task<IEnumerable<Semester>> GetSemesters();
        Task<Semester?> GetSemesterById(int id);

        Task<bool> AddSemester(SemesterViewModel semesterForm);

        Task<bool> UpdateSemester(Semester semester);

        Task<bool> DeleteSemester(Semester semester);
    }
}

using AttendanceAPI.DTO;
using AttendanceAPI.Models;

namespace AttendanceAPI.Repository
{
    public interface IAttendanceRecordRepository
    {
        Task<IEnumerable<AttendanceRecord>> GetAllAsync();
        Task<IEnumerable<AttendanceRecord>> GetAsync(int id);
        Task<IEnumerable<AttendanceRecord>> GetAttendance(AttendanceRecordGetDTO attendanceRequest);
        Task<bool> CreateAsync(AttendanceRecord record);

        Task AddRangeAsync(IEnumerable<AttendanceRecord> records);
        Task<bool> UpdateAsync(AttendanceRecord record);
        Task<bool> DeleteAsync(int id);

        Task SaveAsync();

        Task<bool> CheckIfAlreadyExists();
    }
}

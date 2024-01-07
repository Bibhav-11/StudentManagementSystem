using AttendanceAPI.Models;

namespace AttendanceAPI.Repository
{
    public interface IAttendanceRecordRepository
    {
        Task<IEnumerable<AttendanceRecord>> GetAllAsync();
        Task<AttendanceRecord?> GetAsync(int id);
        Task<bool> CreateAsync(AttendanceRecord record);

        Task AddRangeAsync(IEnumerable<AttendanceRecord> records);
        Task<bool> UpdateAsync(AttendanceRecord record);
        Task<bool> DeleteAsync(int id);

        Task SaveAsync();
    }
}

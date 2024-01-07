using AttendanceAPI.Models;

namespace AttendanceAPI.Service
{
    public interface IAttendanceService
    {
        Task<bool> CreateAsync(AttendanceRecord attendanceRecord);
        Task CreateListOfAttendancesAsync(IEnumerable<AttendanceRecord> attendanceRecords);
        Task<AttendanceRecord?> GetAsync(int id);
        Task<IEnumerable<AttendanceRecord>> GetAllAsync();  
        Task<bool> UpdateAsync(AttendanceRecord attendanceRecord);
        Task<bool> DeleteAsync(int id);
    }
}

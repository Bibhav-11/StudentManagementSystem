using AttendanceAPI.DTO;
using AttendanceAPI.Models;

namespace AttendanceAPI.Service
{
    public interface IAttendanceService
    {
        Task<bool> CreateAsync(AttendanceRecord attendanceRecord);
        Task CreateListOfAttendancesAsync(IEnumerable<AttendanceRecord> attendanceRecords);
        Task<IEnumerable<AttendanceRecord>> GetAsync(int id);
        Task<IEnumerable<AttendanceRecord>> GetAllAsync();  
        Task<bool> UpdateAsync(AttendanceRecord attendanceRecord);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckIfAlreadyExists();

        Task<IEnumerable<AttendanceRecord>> GetAttendance(AttendanceRecordGetDTO attendanceRequests);
    }
}

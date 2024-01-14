using AttendanceAPI.DTO;
using AttendanceAPI.Models;

namespace AttendanceAPI.Repository
{
    public interface IAttendanceRecordRepository
    {
        Task<IEnumerable<AttendanceRecord>> GetAllAsync(int classId);
        Task<IEnumerable<AttendanceRecord>> GetAsync(int id);
        Task<IEnumerable<AttendanceRecord>> GetAttendance(AttendanceRecordGetDTO attendanceRequest);

        //NEW
        Task<IEnumerable<AttendanceRecord>> GetAttendancesForAStudent(int studentId);
        Task<bool> CreateAsync(AttendanceRecord record);

        Task AddRangeAsync(IEnumerable<AttendanceRecord> records);
        Task<bool> UpdateAsync(AttendanceRecord record);
        Task<bool> DeleteAsync(int id);

        Task SaveAsync();

        Task<bool> CheckIfAlreadyExists(int classid);

        Task DeleteRangeAsync(IEnumerable<AttendanceRecord> attendanceRecords);
    }
}

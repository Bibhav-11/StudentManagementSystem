using AttendanceAPI.Data;
using AttendanceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAPI.Repository
{
    public class AttendanceRecordRepository : IAttendanceRecordRepository
    {
        private readonly AttendanceContext _attendanceContext;

        public AttendanceRecordRepository(AttendanceContext attendanceContext)
        {
            _attendanceContext = attendanceContext;
        }

        public async Task<bool> CreateAsync(AttendanceRecord record)
        {
            await _attendanceContext.AttendanceRecords.AddAsync(record);
            await SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var record = await _attendanceContext.AttendanceRecords.FindAsync(id);
            if (record is null) return false;
            if (record != null) _attendanceContext.AttendanceRecords.Remove(record);
            await SaveAsync();
            return true;
        }

        public async Task<AttendanceRecord?> GetAsync(int id)
        {
            var record = await _attendanceContext.AttendanceRecords.FindAsync(id);
            return record;
        }

        public async Task<IEnumerable<AttendanceRecord>> GetAllAsync()
        {
            var records = _attendanceContext.AttendanceRecords.FromSql<AttendanceRecord>($"EXEC AttendanceRecords_GetList");
            return records;
        }

        //TODO Serilog
        public async Task SaveAsync()
        {
            try
            {
                await _attendanceContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(AttendanceRecord record)
        {
            var currentRecord = await _attendanceContext.AttendanceRecords.FindAsync(record.Id);
            if(currentRecord != null) { 
                currentRecord.Id = record.Id;
                currentRecord.AttendanceDate = record.AttendanceDate;
                currentRecord.StudentId = record.StudentId;
                currentRecord.TeacherId = record.TeacherId;

                await SaveAsync();
                return true;
            }
            else return false;
        }

        public async Task AddRangeAsync(IEnumerable<AttendanceRecord> records)
        {
            _attendanceContext.AddRange(records);
            await _attendanceContext.SaveChangesAsync();
        }
    }
}

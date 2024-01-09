using AttendanceAPI.Data;
using AttendanceAPI.DTO;
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

        public async Task<IEnumerable<AttendanceRecord>> GetAsync(int id)
        {
            var record = await _attendanceContext.AttendanceRecords.Where(ar => ar.StudentId == id).ToListAsync();
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

        public async Task<bool> CheckIfAlreadyExists()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var alreadyExists = await _attendanceContext.AttendanceRecords.AnyAsync(a => a.AttendanceDate == today);
            return alreadyExists;
        }

        public async Task<IEnumerable<AttendanceRecord>> GetAttendance(AttendanceRecordGetDTO attendanceRequest)
        {
            var records = await _attendanceContext.AttendanceRecords.FromSql<AttendanceRecord>($"EXEC AttendanceRecords_GetList @StudentId = {attendanceRequest.StudentId}, @TeacherId = {attendanceRequest.TeacherId}, @StartDate = {attendanceRequest.StartDate}, @EndDate={attendanceRequest.EndDate}").ToListAsync();
            return records;
        }
    }
}

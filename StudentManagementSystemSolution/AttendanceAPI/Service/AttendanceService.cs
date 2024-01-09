using AttendanceAPI.DTO;
using AttendanceAPI.Models;
using AttendanceAPI.Repository;

namespace AttendanceAPI.Service
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRecordRepository _recordRepository;

        public AttendanceService(IAttendanceRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public async Task<bool> CreateAsync(AttendanceRecord attendanceRecord)
        {
            try
            {
                return await _recordRepository.CreateAsync(attendanceRecord);
            }
            catch (Exception ex)
            {
                //Logging Exception
                throw;
            }
        }

        public async Task CreateListOfAttendancesAsync(IEnumerable<AttendanceRecord> attendanceRecords)
        {
            try
            {
                await _recordRepository.AddRangeAsync(attendanceRecords);
            }
            catch (Exception ex)
            {
                //Logging Exception
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _recordRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AttendanceRecord>> GetAllAsync()
        {
            try
            {
                return await _recordRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AttendanceRecord>> GetAsync(int id)
        {
            try
            {
                return await _recordRepository.GetAsync(id);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(AttendanceRecord attendanceRecord)
        {
            try
            {
                return await _recordRepository.UpdateAsync(attendanceRecord);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CheckIfAlreadyExists()
        {
            return await _recordRepository.CheckIfAlreadyExists();
        }


        public async Task<IEnumerable<AttendanceRecord>> GetAttendance(AttendanceRecordGetDTO attendanceRequests)
        {
            return await _recordRepository.GetAttendance(attendanceRequests);
        }
    }
}

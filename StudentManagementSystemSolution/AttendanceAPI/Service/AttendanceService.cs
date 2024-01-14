﻿using AttendanceAPI.DTO;
using AttendanceAPI.Migrations;
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

        public async Task<IEnumerable<AttendanceRecord>> GetAllAsync(int classId)
        {
            try
            {
                return await _recordRepository.GetAllAsync(classId);
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

        public async Task<IEnumerable<AttendanceRecord>> GetAttendancesForAStudent(int studentId)
        {
            try
            {
                return await _recordRepository.GetAttendancesForAStudent(studentId);
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public async Task<bool> CheckIfAlreadyExists(int classid)
        {
            return await _recordRepository.CheckIfAlreadyExists(classid);
        }


        public async Task<IEnumerable<AttendanceRecord>> GetAttendance(AttendanceRecordGetDTO attendanceRequests)
        {
            return await _recordRepository.GetAttendance(attendanceRequests);
        }

        public async Task DeleteAllAttendanceOfAStudent(IEnumerable<AttendanceRecord> attendanceRecords)
        {
            await _recordRepository.DeleteRangeAsync(attendanceRecords);
        }
    }
}

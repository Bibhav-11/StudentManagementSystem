using AttendanceAPI.Models;

namespace AttendanceAPI.DTO
{
    public class AttendanceRecordRequest
    {
        public int? StudentId { get; set; }
        public int? TeacherId { get; set; }
        public int? ClassId { get; set; }
        public DateOnly? AttendanceDate { get; set; }
        public int IsPresent { get; set; }

        public AttendanceRecord ToAttendanceRecord(int teacherId, int classId)
        {
            var attendanceRecord = new AttendanceRecord
            {
                StudentId = this.StudentId.Value,
                TeacherId = teacherId,
                AttendanceDate = this.AttendanceDate,
                IsPresent = this.IsPresent,
                ClassId = classId
            };
            return attendanceRecord;
        }
    }
}

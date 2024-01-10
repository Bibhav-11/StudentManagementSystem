using AttendanceAPI.Models;

namespace AttendanceAPI.DTO
{
    public class AttendanceRecordRequest
    {
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public int ClassId { get; set; }
        public DateOnly? AttendanceDate { get; set; }
        public int IsPresent { get; set; }

        public AttendanceRecord ToAttendanceRecord()
        {
            var attendanceRecord = new AttendanceRecord
            {
                StudentId = this.StudentId,
                TeacherId = this.TeacherId,
                AttendanceDate = this.AttendanceDate,
                IsPresent = this.IsPresent,
                ClassId = this.ClassId
            };
            return attendanceRecord;
        }
    }
}

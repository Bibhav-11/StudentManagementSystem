using AttendanceAPI.DTO;

namespace AttendanceAPI.Models
{
    public class AttendanceRecord
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public DateOnly? AttendanceDate { get; set; }
        public int IsPresent { get; set; }
        public int ClassId { get; set; }
        public AttendanceRecordResponse ToAttendanceRecordResponse()
        {
            var attendanceRecordResponse = new AttendanceRecordResponse
            {
                Id = Id,
                StudentId = StudentId,
                TeacherId = TeacherId,
                AttendanceDate = AttendanceDate,
                IsPresent = IsPresent,
                ClassId = ClassId
            };
            return attendanceRecordResponse;
        }
    }
}

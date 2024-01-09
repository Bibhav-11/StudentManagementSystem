using System.ComponentModel.DataAnnotations;

namespace AttendanceAPI.DTO
{
    public class AttendanceRecordGetDTO
    {
        public int? StudentId { get; set; }
        public int? TeacherId { get; set;}


        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }
}

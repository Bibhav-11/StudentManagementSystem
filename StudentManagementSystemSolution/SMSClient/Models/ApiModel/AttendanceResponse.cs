namespace SMSClient.Models.ApiModel
{
    public class AttendanceResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public DateOnly? AttendanceDate { get; set; }
        public string Status { get; set; } = null!;
    }
}

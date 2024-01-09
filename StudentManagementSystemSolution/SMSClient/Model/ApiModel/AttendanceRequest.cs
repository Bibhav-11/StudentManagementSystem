namespace SMSClient.Model.ApiModel
{
    public class AttendanceRequest
    {
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public DateOnly? AttendanceDate { get; set; }
        public int IsPresent { get; set; }
    }
}

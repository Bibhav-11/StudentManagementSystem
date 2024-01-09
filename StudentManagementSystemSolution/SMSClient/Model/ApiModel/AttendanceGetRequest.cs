namespace SMSClient.Model.ApiModel
{
    public class AttendanceGetRequest
    {
        public int? StudentId { get; set; }
        public int? TeacherId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set;}
    }
}

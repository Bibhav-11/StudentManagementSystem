namespace SMSClient.Model.ApiModel
{
    public class AttendanceResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public int ClassId { get; set; }
        public DateOnly? AttendanceDate { get; set; }
        public int IsPresent { get; set; }
        public string Status { get; set; }
    }
}

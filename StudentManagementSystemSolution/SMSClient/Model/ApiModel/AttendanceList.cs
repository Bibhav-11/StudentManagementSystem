namespace SMSClient.Model.ApiModel
{
    public class AttendanceList
    {
        public int Id { get; set; }
        public string Student { get; set; }
        public string MarkedBy { get; set; }
        public DateOnly? Date { get; set; }
        public string Status { get; set; }
    }
}

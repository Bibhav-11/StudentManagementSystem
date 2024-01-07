namespace SMSClient.Models.ViewModel
{
    public class CourseSemesterViewModel
    {
        public int DepartmentId { get; set; }
        public int SemesterId { get; set; }
        public List<string> CourseIds { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace SMSClient.Models
{
    public class SemesterCourse
    {
        public int Id { get; set; } 

        [ForeignKey("Semester")]
        public int SemesterId { get; set; }

        [ForeignKey("Course")]
        public string CourseCode { get; set; } = null!;

        public Semester Semester { get; set; } = null!;
        public Course Course { get; set; } = null!;
        

    }
}

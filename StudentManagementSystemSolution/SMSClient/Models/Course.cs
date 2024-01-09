using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMSClient.Models
{
    public class Course
    {
        [Key]
        public string CourseCode { get; set; } = null!;
        [Required]
        [DisplayName("Short Name")]
        public string ShortName { get; set; } = null!;
        [Required]
        [DisplayName("Long Name")]
        public string LongName { get; set; } = null!;
        [Required]
        public int Credits { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        public Departments? Department { get; set; } = null!;
        public List<SemesterCourse> SemesterCourses { get; } = new List<SemesterCourse>();
        public List<Semester> Semesters { get; } = new List<Semester>();
    }
}

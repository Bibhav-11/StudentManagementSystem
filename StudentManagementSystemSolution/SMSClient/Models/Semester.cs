using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMSClient.Models
{
    public class Semester
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Short Name")]
        public string ShortName { get; set; } = null!;
        [Required]
        [DisplayName("Long Name")]
        public string LongName { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<Departments> Departments { get; } = new List<Departments>();
        public List<Course> Courses { get; } = new List<Course>();
        public List<DepartmentSemester> DepartmentSemesters { get; } = new List<DepartmentSemester>();
        public List<SemesterCourse> SemesterCourses { get; } = new List<SemesterCourse>();
        public List<Student> Students { get; } = new List<Student>();
    }
}

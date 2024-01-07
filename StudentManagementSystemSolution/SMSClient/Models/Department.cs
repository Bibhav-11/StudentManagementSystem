using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMSClient.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Short Name")]
        public string ShortName { get; set; } = null!;
        [Required]
        [DisplayName("Long Name")]
        public string LongName { get; set; } = null!;
        [Required]
        [DisplayName("Email Address")]
        public string Email { get; set; } = null!;
        
        public List<Course> Courses { get; } = new List<Course>();
        public List<Semester> Semesters { get; } = new List<Semester>();
        public List<DepartmentSemester> DepartmentSemesters { get; } = new List<DepartmentSemester>();
        public List<Student> Students { get;  } = new List<Student>();

    }
}

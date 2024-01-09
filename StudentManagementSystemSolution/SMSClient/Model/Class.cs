using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMSClient.Model
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [DisplayName("Start Period")]
        public DateTime StartDate { get; set; }
        [DisplayName("End Period")]
        public DateTime EndDate { get; set; }

        [ForeignKey("Department")]
        [DisplayName("Department")]
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; } = null!;
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    }
}

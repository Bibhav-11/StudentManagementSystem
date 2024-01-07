using System.ComponentModel.DataAnnotations.Schema;

namespace SMSClient.Models
{
    public class DepartmentSemester
    {
        public int Id { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        [ForeignKey("Semester")]
        public int SemesterId { get; set; }

        public Department Department { get; set; } = null!;
        public Semester Semester { get; set; } = null!;

    }
}

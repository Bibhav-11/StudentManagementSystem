using SMSClient.Models.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMSClient.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = null!;

        [Required]
        [DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [DisplayName("Email Address")]
        public string Email { get; set; } = null!;

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        [ForeignKey("Semester")]
        public int? SemesterId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }

        public Departments? Department { get; set; }
        public Semester? Semester { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMSClient.Models.ViewModel
{
    public class DepartmentViewModel
    {
        [Required]
        [DisplayName("Short Name")]
        public string ShortName { get; set; } = null!;
        [Required]
        [DisplayName("Long Name")]
        public string LongName { get; set; } = null!;
        [Required]
        [DisplayName("Email Address")]
        public string Email { get; set; } = null!;

        [DisplayName("Add Courses")]
        public List<string>? CourseCodes { get; set; } = new();

        [DisplayName("Add Semesters")]
        public List<int>? SemesterIds { get; set; } = new();

        [DisplayName("Add Students")]
        public List<int>? StudentIds { get; set; } = new(); 
    }
}

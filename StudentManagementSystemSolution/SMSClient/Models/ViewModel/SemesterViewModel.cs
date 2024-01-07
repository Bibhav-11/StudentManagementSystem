using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SMSClient.Models.ViewModel
{
    public class SemesterViewModel
    {
        [Required]
        [DisplayName("Short Name")]
        public string ShortName { get; set; } = null!;
        [Required]
        [DisplayName("Long Name")]
        public string LongName { get; set; } = null!;
        [Required]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        [DisplayName("Add Courses")]
        public List<string>? CourseCodes { get; set; } = new();

        [DisplayName("Add Departments")]
        public List<int>? DepartmentIds { get; set; } = new();

        [DisplayName("Add Students")]
        public List<int>? StudentIds { get; set; } = new();
    }
}

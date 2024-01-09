using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMSClient.Model
{
    public class Course
    {
        [Key]
        public string CourseCode { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string LongName { get; set; } = null!;

        [ForeignKey("Class")]
        public int? ClassId { get; set; }
        public Class? Class { get; set; }
    }
}

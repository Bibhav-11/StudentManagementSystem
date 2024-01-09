using SMSClient.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMSClient.Model
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}

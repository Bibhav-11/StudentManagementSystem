using System.ComponentModel;

namespace SMSClient.Models.ViewModel
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        public int UserInfoId { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; } = null!;

        [DisplayName("User Name")]
        public string UserName { get; set; } = null!;
        [DisplayName("Email Address")]
        public string Email { get; set; } = null!;
        [DisplayName("Gender")]
        public string Gender { get; set; } = null!;
        [DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }

    }
}

using Microsoft.AspNetCore.Identity;

namespace SMSClient.Model
{
    public class ApplicationUser: IdentityUser
    {
        public UserInfo? UserInfo { get; set; }
        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }
    }
}

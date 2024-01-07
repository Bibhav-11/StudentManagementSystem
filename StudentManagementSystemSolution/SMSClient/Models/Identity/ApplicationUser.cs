using Microsoft.AspNetCore.Identity;

namespace SMSClient.Models.Identity
{
    public class ApplicationUser: IdentityUser
    {
        public UserInfo UserInfo { get; set; }
        public Student Student { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMSClient.Model
{
    public class UserFormViewModel
    {
        public string? Id { get; set; }
        //IdentityUser
        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }


        //UserInfo
        [DisplayName("Full Name")]
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [DisplayName("Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        //Roles
        public List<string>? Roles { get; set; }

        //public string Role { get; set; }

    }
}

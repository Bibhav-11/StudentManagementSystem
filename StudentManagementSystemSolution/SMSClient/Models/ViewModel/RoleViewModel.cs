using System.ComponentModel.DataAnnotations;

namespace SMSClient.Models.ViewModel
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Role Name is Required")]
        public string Name { get; set; }

        public List<CustomRoleClaims> CustomRoleClaims { get; set; }
        public List<bool> IsSelected { get; set; } = new List<bool>();
    }
}

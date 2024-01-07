using SMSClient.Models;
using SMSClient.Models.Identity;
using SMSClient.Models.ViewModel;

namespace SMSClient.Service.Users
{
    public interface IUsersService
    {
        List<ApplicationUser> GetAllUsers();
        Task<ApplicationUser> GetUserById(string userId);
        Task<IList<ApplicationUser>> GetUsersinRole(string roleName);
        Task<List<object>> GetUserInfo(string userId);
        Task<UserInfo?> GetUserInfoValue(string userId);
        Task<IEnumerable<object>> GetAdditionalInfo(string userId);

        Task<bool> CreateUserAsync(UserFormViewModel userForm);

        Task UpdateUserAsync(UserEditViewModel userForm);

        Task DeleteUserAsync(string userId);
    }
}

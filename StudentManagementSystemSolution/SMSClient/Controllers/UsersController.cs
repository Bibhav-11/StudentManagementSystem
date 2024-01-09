using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Serilog;
using SMSClient.Authentication;
using SMSClient.Constants;
using SMSClient.Data.Identity;
using SMSClient.Model;
using SMSClient.Service.Users;
using System.Reflection;
using System.Text.Json;

namespace SMSClient.Controllers
{
    public class UsersController: Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUsersService _usersService;

        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IUsersService usersService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _usersService = usersService;
        }

        [CustomAuthorize(AccessLevels.View, Modules.User)]
        [HttpGet]
        public IActionResult Index()
        {
            var users = _usersService.GetAllUsers();
            return View(users);
        }

        [CustomAuthorize(AccessLevels.Create, Modules.User)]
        [HttpGet]
        public IActionResult ShowCreateUserModal()
        {
            return PartialView("_CreateUserModal");
        }

        [CustomAuthorize(AccessLevels.Create, Modules.User)]
        [HttpPost]
        public async Task<JsonResult> AjaxPost(UserFormViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "The provided information is not correct" });
                }
                if (user.Password != user.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Password is not same");
                    return Json(new { success = false, message = "Password is not same" });
                }
                var result = await _usersService.CreateUserAsync(user);
                if (result)
                {
                    Log.Information("Successfully created the user");
                    return Json(new { success = true, message = "Added Successfully" });
                }
                else return Json(new { success = false, message = "Sorry some error occured" });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Create, Modules.User)]
        [HttpGet]
        public async Task<IActionResult> ShowUpdateUserModal(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return NotFound();
                }
                var userInfo = await _usersService.GetUserInfoValue(userId);
                var firstRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                var userViewModel = new UserEditViewModel
                {
                    Id = userId,
                    Email = user.Email,
                    UserName = user.UserName,
                    FullName = "",
                    DateOfBirth = DateTime.Now,
                    Gender = "",
                };
                if (userInfo != null)
                {
                    userViewModel.FullName = userInfo.FullName;
                    userViewModel.DateOfBirth = userInfo.DateOfBirth;
                    userViewModel.Gender = userInfo.Gender;
                    userViewModel.UserInfoId = userInfo.Id;
                }

                return PartialView("_EditUserModal", userViewModel);
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Create, Modules.User)]
        [HttpPost]
        public async Task<JsonResult> AjaxUpdate(UserEditViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "The provided information is not correct" });
                }

                await _usersService.UpdateUserAsync(user);
                Log.Information("Successfully updated the user");
                return Json(new { success = true, message = "Added Successfully" });
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.User)]
        [HttpGet]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user is null) return NotFound();

                var userInfo = await _usersService.GetUserInfoValue(userId);
                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                var userViewModel = new UserFormViewModel
                {
                    Id = userId,
                    FullName = "",
                    UserName = user.UserName,
                };
                if (userInfo != null)
                {
                    userViewModel.FullName = userInfo.FullName;
                }

                return PartialView("_DeleteUserModal", userViewModel);
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.User)]
        [HttpPost]
        public async Task<JsonResult> DeleteUser(string userId)
        {
            try
            {
                await _usersService.DeleteUserAsync(userId);
                Log.Information("Successfully deleted the user");
                return Json(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetRolesForDropdown()
        {
            var roles = _roleManager.Roles.ToList();
            return Json(roles);
        }

        [HttpGet]
        public async Task<JsonResult> CheckIfUserNameExists(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Json(false);
            }
            else return Json(true);
        }
    }
}

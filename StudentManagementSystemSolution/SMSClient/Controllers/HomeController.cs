using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMSClient.Model;
using SMSClient.Service.Users;

namespace SMSClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(IUsersService usersService, UserManager<ApplicationUser> userManager)
        {
            _usersService = usersService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.AccessToken = await HttpContext.GetTokenAsync("access_token");

            var user = await userManager.GetUserAsync(HttpContext.User);
            var isStudent = await userManager.IsInRoleAsync(user, "student");
            var userInfo = await _usersService.GetAdditionalInfo(user.Id);
            return View(userInfo);
        }

        public async Task LogOut()
        {
            await HttpContext.SignOutAsync("oidc");
        }


    }
}
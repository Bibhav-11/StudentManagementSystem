using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityServer.Data.Migrations;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ProfileService(ILogger<ProfileService> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (context.RequestedClaimTypes.Any())
            {
                context.AddRequestedClaims(context.Subject.Claims);
            }
            
            if(context.Caller == "UserInfoEndpoint")
            {
                if(context.RequestedResources.Resources.IdentityResources.Any(idrs => idrs.Name == "permissions"))
                {

                    var allRolesofCurrentUser = context.Subject.FindAll(JwtClaimTypes.Role).ToList();
                    foreach(var role in allRolesofCurrentUser)
                    {
                        var roleString = role.Value;
                        var identityRole = await _roleManager.FindByNameAsync(roleString);
                        var claimsOfRole = await _roleManager.GetClaimsAsync(identityRole);
                        context.AddRequestedClaims(claimsOfRole);
                    }
                }
            }

            return;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}

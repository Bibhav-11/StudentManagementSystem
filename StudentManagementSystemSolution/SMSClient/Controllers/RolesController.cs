using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using SMSClient.Authentication;
using SMSClient.Constants;
using SMSClient.Models;
using SMSClient.Models.Identity;
using SMSClient.Models.ViewModel;
using System.Security.Claims;

namespace SMSClient.Controllers
{
    public class RolesController: Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [CustomAuthorize(AccessLevels.View, Modules.Role)]
        [HttpGet]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }


        [CustomAuthorize(AccessLevels.Create, Modules.Role)]
        [HttpGet]
        public IActionResult Create()
        {
            var roleModel = new RoleViewModel();
            roleModel.CustomRoleClaims = ClaimsList.CustomRoleClaims;

            return View(roleModel);
        }

        [CustomAuthorize(AccessLevels.Create, Modules.Role)]
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            try
            {
                var allClaims = ClaimsList.CustomRoleClaims;
                var selectedClaimsIndex = roleViewModel.IsSelected;

                var selectedClaims = allClaims.Where((_, index) => selectedClaimsIndex[index]).ToList();

                var role = new IdentityRole(roleViewModel.Name);
                await _roleManager.CreateAsync(role);

                foreach (var claim in selectedClaims)
                {
                    var newClaim = new Claim("permission", claim.Value);
                    await _roleManager.AddClaimAsync(role, newClaim);
                }
                Log.Information("New Role Created");
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Log.Error("Error while creating a role " + ex.Message);
                throw;
            }
        }


        [CustomAuthorize(AccessLevels.Edit, Modules.Role)]
        [HttpGet]
        public async Task<IActionResult> Edit(string? roleId)
        {
            if (roleId == null) return NotFound();
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();
            return View(role);
        }

        [CustomAuthorize(AccessLevels.Edit, Modules.Role)]
        [HttpPost]
        public async Task<IActionResult> Update(IdentityRole role)
        {
            try
            {
                var existingRole = await _roleManager.FindByIdAsync(role.Id);
                existingRole.Name = role.Name;
                await _roleManager.UpdateAsync(existingRole);
                Log.Information("Successfully updated the role");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Log.Error("Could not update the role" + ex.Message);
                throw;
            }
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Role)]
        [HttpGet]
        public async Task<IActionResult> Delete(string? roleId)
        {
            if (roleId == null) return NotFound();
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();
            return PartialView("_DeleteRoleModal", role);
        }

        [CustomAuthorize(AccessLevels.Delete, Modules.Role)]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                await _roleManager.DeleteAsync(role);
                Log.Information("Successfully create the role");
                return Json(true);
            }
            catch(Exception ex)
            {
                Log.Error("Could not delete the role" + ex.Message);
                throw;
            }
        }
    }
}

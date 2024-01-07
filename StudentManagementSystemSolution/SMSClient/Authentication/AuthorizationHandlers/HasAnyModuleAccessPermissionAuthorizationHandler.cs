using Microsoft.AspNetCore.Authorization;
using SMSClient.Constants;
using System.Security;

namespace SMSClient.Authentication.AuthorizationHandlers
{
    public class HasAnyModuleAccessPermissionAuthorizationHandler : AuthorizationHandler<HasAnyModuleAccessPermission>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasAnyModuleAccessPermission requirement)
        {
            var permissionCreate = Permission.Value(AccessLevels.Create, requirement.Module);
            var permissionEdit = Permission.Value(AccessLevels.Edit, requirement.Module);
            var permissionView = Permission.Value(AccessLevels.View, requirement.Module);
            var permissionDelete = Permission.Value(AccessLevels.Delete, requirement.Module);

            var HasAnyPermission = context.User.HasClaim("permission", permissionCreate) || context.User.HasClaim("permission", permissionView) || context.User.HasClaim("permission", permissionEdit) || context.User.HasClaim("permission", permissionDelete);

            if (!context.User.IsInRole("admin"))
            {
                if (!HasAnyPermission)
                {
                    context.Fail();
                }
                else
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

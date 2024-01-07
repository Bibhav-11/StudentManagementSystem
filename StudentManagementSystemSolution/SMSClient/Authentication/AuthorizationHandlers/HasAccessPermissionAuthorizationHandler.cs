using Microsoft.AspNetCore.Authorization;

namespace SMSClient.Authentication.AuthorizationHandlers
{
    public class HasAccessPermissionAuthorizationHandler : AuthorizationHandler<HasAccessPermission>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasAccessPermission requirement)
        {
            var permission = Permission.Value(requirement.AccessLevel, requirement.Module);

            if (!context.User.IsInRole("admin"))
            {
                if (!context.User.HasClaim("permission", permission))
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

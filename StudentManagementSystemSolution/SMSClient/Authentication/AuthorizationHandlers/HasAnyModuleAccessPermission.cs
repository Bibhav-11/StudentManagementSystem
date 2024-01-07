using Microsoft.AspNetCore.Authorization;

namespace SMSClient.Authentication.AuthorizationHandlers
{
    public class HasAnyModuleAccessPermission: IAuthorizationRequirement
    {
        public string Module { get; set; }

        public HasAnyModuleAccessPermission(string module)
        {
            Module = module;
        }
    }
}

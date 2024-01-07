using Microsoft.AspNetCore.Authorization;

namespace SMSClient.Authentication.AuthorizationHandlers
{
    public class HasAccessPermission: IAuthorizationRequirement
    {
        public string AccessLevel { get; set; }
        public string Module { get; set; }  
        public HasAccessPermission(string accesslevel, string module)
        {
            AccessLevel = accesslevel;
            Module = module;
        }
    }
}

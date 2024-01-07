using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace SMSClient.Authentication
{
    public class CustomAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        private string AccessLevel;
        private string Module;
        public CustomAuthorize(string accessLevel, string module)
        {
            AccessLevel = accessLevel;
            Module = module;
        }
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var permission = Permission.Value(AccessLevel, Module);

            if(!filterContext.HttpContext.User.IsInRole("admin"))
            {
                if (!filterContext.HttpContext.User.HasClaim("permission", permission))
                {
                    filterContext.Result = new ForbidResult();
                }
            }
           
        }
    }
}
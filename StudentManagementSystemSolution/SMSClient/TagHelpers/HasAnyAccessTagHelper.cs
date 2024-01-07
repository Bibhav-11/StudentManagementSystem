using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SMSClient.Authentication.AuthorizationHandlers;
using SMSClient.Constants;

namespace SMSClient.TagHelpers
{
    [HtmlTargetElement("*", Attributes = "asp-hasanyaccessmodule")]
    public class HasAnyAccessTagHelper: TagHelper
    {
        private readonly IAuthorizationService _authorizationService;

        public HasAnyAccessTagHelper(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HtmlAttributeName("asp-hasanyaccessmodule")]
        public string Module { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);

            var result = ViewContext.HttpContext.User.IsInRole("admin");

            var isAuthorized = await _authorizationService.AuthorizeAsync(ViewContext.HttpContext.User, context.Items, new HasAnyModuleAccessPermission(Module));


            if (!isAuthorized.Succeeded) output.SuppressOutput();

        }
    }
}

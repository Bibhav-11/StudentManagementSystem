using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SMSClient.Authentication;
using SMSClient.Authentication.AuthorizationHandlers;

namespace SMSClient.TagHelpers
{
    [HtmlTargetElement("*", Attributes = "asp-accesslevel,asp-module")]
    public class CustomTagHelper: TagHelper
    {
        private readonly IAuthorizationService _authorizationService;

        public CustomTagHelper(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HtmlAttributeName("asp-accesslevel")]
        public string AccessLevel { get; set; }

        [HtmlAttributeName("asp-module")]
        public string Module { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);

            var isAuthorized = await _authorizationService.AuthorizeAsync(ViewContext.HttpContext.User, context.Items, new HasAccessPermission(AccessLevel, Module));

            if (!isAuthorized.Succeeded) output.SuppressOutput();

        }
    }
}

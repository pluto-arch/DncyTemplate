using Dncy.Permission;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DncyTemplate.Mvc.TagHelpers
{
    [HtmlTargetElement(Attributes = "asp-authorize")]
    [HtmlTargetElement(Attributes = "asp-authorize,asp-permission-code")]
    public class AuthorizeTagHelper : TagHelper
    {
        private readonly IPermissionChecker _permissionChecker;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizeTagHelper(IPermissionChecker permissionChecker, IHttpContextAccessor httpContextAccessor)
        {
            _permissionChecker = permissionChecker;
            _httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// Gets or sets the policy name that determines access to the HTML block.
        /// </summary>
        [HtmlAttributeName("asp-permission-code")]
        public string PermissionCode { get; set; }


        /// <inheritdoc />
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && !string.IsNullOrEmpty(PermissionCode))
            {
                var grantRes = await _permissionChecker.IsGrantedAsync(user, PermissionCode);
                if (!grantRes)
                {
                    output.SuppressOutput();
                }
            }
        }
    }
}
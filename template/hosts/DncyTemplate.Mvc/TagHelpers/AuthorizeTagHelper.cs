using Dotnetydd.Permission;
using Dotnetydd.Permission.Models;
using DncyTemplate.Application.Models.Application.Navigation;
using DncyTemplate.Mvc.Models.Account;
using Dotnetydd.Permission.Checker;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DncyTemplate.Mvc.TagHelpers
{
    [HtmlTargetElement(Attributes = "asp-authorize")]
    [HtmlTargetElement(Attributes = "asp-authorize,asp-permission")]
    public class AuthorizeTagHelper : TagHelper
    {
        private readonly IPermissionChecker _permissionChecker;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizeTagHelper(IPermissionChecker permissionChecker, IHttpContextAccessor httpContextAccessor)
        {
            _permissionChecker = permissionChecker;
            _httpContextAccessor = httpContextAccessor;
        }


        [HtmlAttributeName("asp-permission")]
        public MenuPermission MenuPermission { get; set; }


        /// <inheritdoc />
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                output.SuppressOutput();
                return;
            }

            if (MenuPermission.SkipCheck)
            {
                await base.ProcessAsync(context, output);
                return;
            }

            if (user.IsInRole(RoleEnum.SA.ToString()))
            {
                await base.ProcessAsync(context, output);
                return;
            }


            if (MenuPermission == null)
            {
                output.SuppressOutput();
                return;
            }

            var grantRes = await _permissionChecker.IsGrantedAsync(user, MenuPermission.PermissionCode);
            if (MenuPermission.RequiredAll)
            {
                if (!grantRes.AllGranted)
                {
                    output.SuppressOutput();
                }
            }
            else
            {
                if (grantRes.Result.All(x => x.Value == PermissionGrantResult.Prohibited || x
                        .Value == PermissionGrantResult.Undefined))
                {
                    output.SuppressOutput();
                }
            }
        }
    }
}
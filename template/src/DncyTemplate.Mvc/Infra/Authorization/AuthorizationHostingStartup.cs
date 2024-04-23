using System.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace DncyTemplate.Mvc.Infra.Authorization;

public static class AuthorizationHostingStartup
{
    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();

        services.AddAuthentication(config =>
            {
                config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
                options.SlidingExpiration = true;
                options.Cookie.Name = "_app_auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.AccessDeniedPath = new PathString($"/error/403/{HttpUtility.UrlEncode("权限不足")}");
                options.LoginPath = new PathString("/account/login");
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });

        services.AddAuthorization();
    }
}
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Web;

[assembly: HostingStartup(typeof(DncyTemplate.Mvc.Infra.Authorization.AuthorizationHostingStartup))]
namespace DncyTemplate.Mvc.Infra.Authorization;

public class AuthorizationHostingStartup : IHostingStartup
{
    /// <inheritdoc />
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((_, services) =>
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
        });
    }
}
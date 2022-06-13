using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

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

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            services.AddAuthorization();
        });
    }
}
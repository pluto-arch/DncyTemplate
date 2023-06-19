using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

[assembly: HostingStartup(typeof(DncyTemplate.Api.Infra.Authorization.AuthorizationHostingStartup))]
namespace DncyTemplate.Api.Infra.Authorization
{
    public class AuthorizationHostingStartup : IHostingStartup
    {
        /// <inheritdoc />
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((_, services) =>
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = "pluto",
                            ValidAudience = "12312",
                            ClockSkew = TimeSpan.FromMinutes(30),
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("715B59F3CDB1CF8BC3E7C8F13794CEA9"))
                        };
                        options.RequireHttpsMetadata = false;
                    }); // 认证
                services.AddAuthorization();
                services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();
                services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
            });
        }
    }
}
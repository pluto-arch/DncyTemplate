using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace DncyTemplate.ApiEndpoint.Infra.Authorization
{
    public static class AuthorizationHostingStartup
    {
        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "pluto",
                        ValidAudiences = new List<string> 
                        {
                            "a",
                            "b" 
                        },
                        ClockSkew = TimeSpan.FromMinutes(30),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("715B59F3CDB1CF8BC3E7C8F13794CEA9"))
                    };

                    options.IncludeErrorDetails = true;
                    options.RequireHttpsMetadata = false;
                }); // 认证
            services.AddAuthorization();
            services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
        }
    }
}

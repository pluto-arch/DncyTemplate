#if Tenant
using Dotnetydd.MultiTenancy.AspNetCore;

namespace DncyTemplate.ApiEndpoint.Infra.Tenancy
{
    public static class TenancyApplicationBuilderExtension
    {
        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
        {
            app.UseMiddleware<MultiTenancyMiddleware>();
            return app;
        }
    }
}
#endif

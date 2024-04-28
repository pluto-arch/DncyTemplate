#if Tenant

using Dotnetydd.MultiTenancy;
using Dotnetydd.MultiTenancy.AspNetCore;
using Dotnetydd.MultiTenancy.AspNetCore.TenantIdentityParse;
using Dotnetydd.MultiTenancy.Store;



namespace DncyTemplate.BlazorServer.Infra.Tenancy
{
    public static class TenancyHostingStartup
    {
        public static void ConfigureTenancy(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TenantConfigurationOptions>(configuration);
            services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
            services.AddTransient<ICurrentTenant, CurrentTenant>();
            services.AddTransient<IConnectionStringResolver, DefaultConnectionStringResolver>();
            services.AddTransient<ITenantStore, DefaultTenantStore>();
            services.AddTransient<ITenantResolver, TenantResolver>();
            services.AddTransient<ITenantIdentityParse, UserTenantIdentityParse>();
            services.AddTransient<MultiTenancyMiddleware>();
        }
    }
}

#endif

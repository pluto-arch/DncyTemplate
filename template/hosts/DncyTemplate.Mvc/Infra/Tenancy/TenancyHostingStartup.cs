
#if Tenant
using Dncy.MultiTenancy;
using Dncy.MultiTenancy.AspNetCore;
using Dncy.MultiTenancy.ConnectionStrings;
using Dncy.MultiTenancy.Store;

namespace DncyTemplate.Mvc.Infra.Tenancy;

public static class TenancyHostingStartup
{
    /// <inheritdoc />
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
#endif
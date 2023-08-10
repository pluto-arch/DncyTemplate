using Dncy.MultiTenancy;
using Dncy.MultiTenancy.AspNetCore;
using Dncy.MultiTenancy.ConnectionStrings;
using Dncy.MultiTenancy.Store;

#if Tenant
[assembly: HostingStartup(typeof(DncyTemplate.Api.Infra.Tenancy.TenancyHostingStartup))]
namespace DncyTemplate.Api.Infra.Tenancy;

public class TenancyHostingStartup : IHostingStartup
{
    /// <inheritdoc />
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            services.Configure<TenantConfigurationOptions>(context.Configuration);
            services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
            services.AddTransient<ICurrentTenant, CurrentTenant>();
            services.AddTransient<IConnectionStringResolver, DefaultConnectionStringResolver>();
            services.AddTransient<ITenantStore, DefaultTenantStore>();
            services.AddTransient<ITenantResolver, TenantResolver>();
            services.AddTransient<ITenantIdentityParse, UserTenantIdentityParse>();
            services.AddTransient<MultiTenancyMiddleware>();
        });
    }
}
#endif
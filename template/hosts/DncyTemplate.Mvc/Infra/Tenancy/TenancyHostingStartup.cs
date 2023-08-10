
#if Tenant
using Dncy.MultiTenancy;
using Dncy.MultiTenancy.AspNetCore;
using Dncy.MultiTenancy.ConnectionStrings;
using Dncy.MultiTenancy.Store;

[assembly: HostingStartup(typeof(DncyTemplate.Mvc.Infra.Tenancy.TenancyHostingStartup))]
namespace DncyTemplate.Mvc.Infra.Tenancy;

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
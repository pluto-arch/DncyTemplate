
#if Tenant
using Dncy.MultiTenancy;
using Dncy.MultiTenancy.ConnectionStrings;
#endif
using DncyTemplate.Infra.Constants;

namespace DncyTemplate.Infra.EntityFrameworkCore.ConnectionStringResolve;

public class DefaultConnectionStringResolve : IConnectionStringResolve
{
    protected readonly IConfiguration _configuration;
#if Tenant
    private readonly ICurrentTenant _currentTenant;

    
    protected readonly TenantConfigurationOptions _tenantConfigurationOptions;
#endif
    public DefaultConnectionStringResolve(
        IConfiguration configuration
#if Tenant
        ,ICurrentTenant currentTenant,
        IOptions<TenantConfigurationOptions> options
#endif
        )
    {
        _configuration = configuration;
#if Tenant
        _currentTenant = currentTenant;
        _tenantConfigurationOptions = options?.Value;
#endif
        
    }

    public virtual Task<string> GetAsync(string connectionStringName = null)
    {
        connectionStringName ??= DbConstants.DEFAULT_CONNECTIONSTRING_NAME;
        var defaultConnectionString = _configuration.GetConnectionString(connectionStringName);
#if Tenant
        var tenan = _currentTenant;
        if (tenan == null)
            return Task.FromResult(defaultConnectionString);


        if (_tenantConfigurationOptions == null || _tenantConfigurationOptions.Tenants == null)
            return Task.FromResult(defaultConnectionString);

        var t = _tenantConfigurationOptions.Tenants.FirstOrDefault(x => x.TenantId == tenan.Id);
        if (t == null)
            return Task.FromResult(defaultConnectionString);
        var connStr = t.ConnectionStrings?.FirstOrDefault(x => x.Key == connectionStringName).Value;
         return Task.FromResult(connStr ?? defaultConnectionString);
#else
        return Task.FromResult(defaultConnectionString);
#endif
    }
}
using Dncy.MultiTenancy;
using Dncy.MultiTenancy.ConnectionStrings;

using DncyTemplate.Infra.Constants;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DncyTemplate.Infra.EntityFrameworkCore.ConnectionStringResolve;

public class DefaultConnectionStringResolve : IConnectionStringResolve
{
    protected readonly IConfiguration _configuration;
    private readonly ICurrentTenant _currentTenant;
    protected readonly TenantConfigurationOptions _tenantConfigurationOptions;

    public DefaultConnectionStringResolve(
        IConfiguration configuration,
        ICurrentTenant currentTenant,
        IOptions<TenantConfigurationOptions> options)
    {
        _configuration = configuration;
        _currentTenant = currentTenant;
        _tenantConfigurationOptions = options?.Value;
    }

    public virtual Task<string> GetAsync(string connectionStringName = null)
    {
        connectionStringName ??= DbConstants.DEFAULT_CONNECTIONSTRING_NAME;
        var defaultConnectionString = _configuration.GetConnectionString(connectionStringName);
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
    }
}
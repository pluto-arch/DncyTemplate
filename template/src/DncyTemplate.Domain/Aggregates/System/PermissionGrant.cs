using DncyTemplate.Domain.Infra;
using Dotnetydd.Permission.PermissionGrant;

namespace DncyTemplate.Domain.Aggregates.System;

public class PermissionGrant : BaseEntity<int>, IPermissionGrant
#if Tenant
    , IMultiTenant
#endif
{

    public PermissionGrant()
    {
        CreateTime = DateTimeOffset.Now;
    }

    public PermissionGrant(string name, string providerName, string providerKey) : this()
    {
        Name = name;
        ProviderName = providerName;
        ProviderKey = providerKey;
    }

    /// <inheritdoc />
    public string Name { get; set; }

    /// <inheritdoc />
    public string ProviderName { get; set; }

    /// <inheritdoc />
    public string ProviderKey { get; set; }

#if Tenant
    /// <inheritdoc />
    public string TenantId { get; set; }
#endif

    public DateTimeOffset CreateTime { get; set; }
}
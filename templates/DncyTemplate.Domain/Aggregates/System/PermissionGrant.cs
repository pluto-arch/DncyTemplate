using Dncy.Permission;
using DncyTemplate.Domain.Infra;

namespace DncyTemplate.Domain.Aggregates.System;

public class PermissionGrant : BaseEntity<int>, IPermissionGrant, IMultiTenant
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

    /// <inheritdoc />
    public string TenantId { get; set; }

    public DateTimeOffset CreateTime { get; set; }
}
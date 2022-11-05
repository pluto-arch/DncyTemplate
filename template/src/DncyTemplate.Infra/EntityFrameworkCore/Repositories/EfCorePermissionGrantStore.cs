using Dncy.Permission;

using DncyTemplate.Domain.Aggregates.System;
using DncyTemplate.Domain.Repository;

namespace DncyTemplate.Infra.EntityFrameworkCore.Repositories;

public class EfCorePermissionGrantStore : IPermissionGrantStore
{
    private readonly IRepository<PermissionGrant> _permissionGrants;

    public EfCorePermissionGrantStore(IRepository<PermissionGrant> repository)
    {
        _permissionGrants = repository;
    }


    /// <inheritdoc />
    public async Task<IPermissionGrant> GetAsync(string name, string providerName, string providerKey)
    {
        return await _permissionGrants.FirstOrDefaultAsync(s => s.ProviderName == providerName && s.ProviderKey == providerKey);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IPermissionGrant>> GetListAsync(string providerName, string providerKey)
    {
        return await _permissionGrants.AsNoTracking().Where(s => s.ProviderName == providerName && s.ProviderKey == providerKey)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IPermissionGrant>> GetListAsync(string[] names, string providerName, string providerKey)
    {
        return await _permissionGrants.AsNoTracking().Where(s => names.Contains(s.Name) && s.ProviderName == providerName && s.ProviderKey == providerKey)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task GrantAsync(string name, string providerName, string providerKey)
    {
        await _permissionGrants.InsertAsync(new PermissionGrant(name, providerName, providerKey), true);
    }

    /// <inheritdoc />
    public async Task GrantAsync(string[] name, string providerName, string providerKey)
    {
        var tempList = name.Select(x => new PermissionGrant(x, providerName, providerKey));
        await _permissionGrants.InsertAsync(tempList, true);
    }

    /// <inheritdoc />
    public async Task CancleGrantAsync(string name, string providerName, string providerKey)
    {
        await _permissionGrants.DeleteAsync(x => x.Name == name && x.ProviderName == providerName && x.ProviderKey == providerKey, true);
    }

    /// <inheritdoc />
    public async Task CancleGrantAsync(string[] name, string providerName, string providerKey)
    {
        await _permissionGrants.DeleteAsync(x => name.Contains(x.Name) && x.ProviderName == providerName && x.ProviderKey == providerKey, true);
    }
}
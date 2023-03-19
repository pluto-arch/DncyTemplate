using Dncy.Permission;
using DncyTemplate.Application.Constants;
using DncyTemplate.Application.Permission.Models;
using System.Transactions;

namespace DncyTemplate.Application.Permission;

[Injectable(InjectLifeTime.Scoped, typeof(IPermissionAppService))]
[AutoResolveDependency]
public partial class PermissionAppService : IPermissionAppService
{
    [AutoInject]
    private readonly IPermissionManager _permissionManager;

    [AutoInject]
    private readonly IPermissionGrantStore _permissionGrantStore;

    [AutoInject]
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;


    public async Task<List<PermissionGroupDto>> GetPermissionsAsync(string providerName, string providerValue)
    {
        var res = new List<PermissionGroupDto>();
        var groups = _permissionDefinitionManager.GetGroups();
        foreach (var item in groups)
        {
            var group = new PermissionGroupDto
            {
                Name = item.Name,
                DisplayName = item.DisplayName,
                Permissions = new List<PermissionDto>()
            };
            var grantModel = item.GetPermissionsWithChildren();
            var grantResult = await _permissionManager.IsGrantedAsync(grantModel.Select(x => x.Name).ToArray(), providerName, providerValue);
            foreach (var permission in item.GetPermissionsWithChildren())
            {
                _ = grantResult.Result.TryGetValue(permission.Name, out var isGranted);
                var permiss = new PermissionDto
                {
                    Name = permission.Name,
                    DisplayName = permission.DisplayName,
                    ParentName = permission.Parent!,
                    IsGrant = isGranted == Dncy.Permission.Models.PermissionGrantResult.Granted,
                    AllowProviders = permission.AllowedProviders.ToArray(),
                };
                group.Permissions.Add(permiss);
            }
            res.Add(group);
        }

        return res;
    }

    /// <inheritdoc />
    public async Task GrantAsync(string[] permissions, string providerName, string providerValue)
    {
        TransactionOptions transactionOption = new TransactionOptions();
        transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
        transactionOption.Timeout = new TimeSpan(0, 0, 120);

        using var scoped = new TransactionScope(TransactionScopeOption.Required, transactionOption);
        var old = await _permissionGrantStore.GetListAsync(providerName, providerValue);
        var names = old.Select(x => x.Name).ToArray();
        if (permissions is { Length: <= 0 })
        {
            await _permissionGrantStore.CancleGrantAsync(names, providerName, providerValue);
            names.AsParallel().ForAll(x =>
            {
                PermissionGrantCache.Cache.AddOrUpdate(string.Format(CacheKeyFormatConstants.Permission_Grant_CacheKey_Format, providerName, providerValue, x), false.ToString(), (k, oldv) => false.ToString());
            });
            return;
        }

        var exp = names.Except(permissions);
        if (exp.Any())
        {
            await _permissionGrantStore.CancleGrantAsync(exp.ToArray(), providerName, providerValue);
            exp.AsParallel().ForAll(x =>
            {
                PermissionGrantCache.Cache.AddOrUpdate(string.Format(CacheKeyFormatConstants.Permission_Grant_CacheKey_Format, providerName, providerValue, x), false.ToString(), (k, oldv) => false.ToString());
            });
        }


        foreach (var grantInfo in permissions)
        {
            if (names.Contains(grantInfo))
            {
                continue;
            }
            var permission = _permissionDefinitionManager.Get(grantInfo);
            if (permission.AllowedProviders.Any() && !permission.AllowedProviders.Contains(providerName))
            {
                throw new ApplicationException($"The permission named {permission.Name} has not compatible with the provider named {providerName}");
            }

            if (!permission.IsEnabled)
            {
                throw new ApplicationException($"The permission named {permission.Name} is disabled");
            }

            await _permissionGrantStore.GrantAsync(grantInfo, providerName, providerValue);
            string cacheKey = string.Format(CacheKeyFormatConstants.Permission_Grant_CacheKey_Format, providerName, providerValue, grantInfo);
            PermissionGrantCache.Cache.AddOrUpdate(cacheKey, true.ToString(), (k, oldv) => true.ToString());
        }

        scoped.Complete();
    }
}
using System.Transactions;
using DncyTemplate.Application.Permission.Models;
using DncyTemplate.Constants;
using Dotnetydd.Permission.Definition;
using Dotnetydd.Permission.PermissionGrant;
using Dotnetydd.Permission.PermissionManager;

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
                Permissions = []
            };
            var grantModel = item.GetPermissionsWithChildren();
            var grantResult = await _permissionManager.IsGrantedAsync(grantModel.Select(x => x.Name).ToArray(), providerName, providerValue);
            foreach (var permissionItem in item.GetPermissionsWithChildren())
            {
                _ = grantResult.Result.TryGetValue(permissionItem.Name, out var isGranted);
                var permission = new PermissionDto
                {
                    Name = permissionItem.Name,
                    DisplayName = permissionItem.DisplayName,
                    ParentName = permissionItem.Parent!,
                    IsGrant = isGranted == Dotnetydd.Permission.Models.PermissionGrantResult.Granted,
                    AllowProviders = [.. permissionItem.AllowedProviders],
                };
                group.Permissions.Add(permission);
            }
            res.Add(group);
        }

        return res;
    }

    /// <inheritdoc />
    public async Task GrantAsync(string[] permissions, string providerName, string providerValue)
    {
        TransactionOptions transactionOption = new()
        {
            IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
            Timeout = new TimeSpan(0, 0, 120),
        };

        using var scoped = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled);
        var old = await _permissionGrantStore.GetListAsync(providerName, providerValue);
        var names = old.Select(x => x.Name).ToArray();
        if (permissions is { Length: <= 0 })
        {
            await _permissionGrantStore.RemoveGrantAsync(names, providerName, providerValue);
            names.AsParallel().ForAll(x =>
            {
                PermissionGrantCache.Cache.AddOrUpdate(string.Format(CacheKeyFormatConstantValue.PERMISSION_GRANT_CACHEKEY_FORMAT, providerName, providerValue, x), false.ToString(), (k, _) => false.ToString());
            });
            return;
        }

        var exp = names.Except(permissions);
        if (exp.Any())
        {
            await _permissionGrantStore.RemoveGrantAsync(exp.ToArray(), providerName, providerValue);
            exp.AsParallel().ForAll(x =>
            {
                PermissionGrantCache.Cache.AddOrUpdate(string.Format(CacheKeyFormatConstantValue.PERMISSION_GRANT_CACHEKEY_FORMAT, providerName, providerValue, x), false.ToString(), (k, _) => false.ToString());
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

            await _permissionGrantStore.SaveAsync(grantInfo, providerName, providerValue);
            string cacheKey = string.Format(CacheKeyFormatConstantValue.PERMISSION_GRANT_CACHEKEY_FORMAT, providerName, providerValue, grantInfo);
            PermissionGrantCache.Cache.AddOrUpdate(cacheKey, true.ToString(), (k, oldv) => true.ToString());
        }

        scoped.Complete();
    }
}
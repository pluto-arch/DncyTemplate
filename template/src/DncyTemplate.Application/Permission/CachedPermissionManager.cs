using DncyTemplate.Constants;
using Dotnetydd.Permission.Definition;
using Dotnetydd.Permission.Models;
using Dotnetydd.Permission.PermissionGrant;
using Dotnetydd.Permission.PermissionManager;


namespace DncyTemplate.Application.Permission;


public class PermissionGrantCache
{
    // TODO ConcurrentDictionary just as an example
    public static readonly ConcurrentDictionary<string, string> Cache = new();
}


[AutoResolveDependency]
public partial class CachedPermissionManager : IPermissionManager
{
    [AutoInject]
    private readonly ILogger<InMemoryPermissionManager> _logger;

    [AutoInject]
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;

    [AutoInject]
    private readonly IPermissionGrantStore _permissionGrantStore;

    private static readonly ConcurrentDictionary<string, string> permissionCached = PermissionGrantCache.Cache;

    /// <inheritdoc />
    public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
    {
        return (await GetCacheItemAsync(name, providerName, providerKey)).IsGranted;
    }


    /// <inheritdoc />
    public async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
    {
        if (names is null)
        {
            throw new ArgumentNullException(nameof(names));
        }

        MultiplePermissionGrantResult result = new MultiplePermissionGrantResult();

        if (names.Length == 1)
        {
            string name = names.First();
            result.Result.Add(name,
                await IsGrantedAsync(names.First(), providerName, providerKey)
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Prohibited);
            return result;
        }

        var cacheItems = await GetCacheItemsAsync(names, providerName, providerKey);

        foreach ((string Key, bool IsGranted) in cacheItems)
        {
            result.Result.Add(GetPermissionInfoFormCacheKey(Key).Name,
                IsGranted ? PermissionGrantResult.Granted : PermissionGrantResult.Prohibited);
        }

        return result;
    }



    #region protected

    protected virtual async Task<(string Key, bool IsGranted)> GetCacheItemAsync(string name, string providerName, string providerKey)
    {
        string cacheKey = string.Format(CacheKeyFormatConstantValue.PERMISSION_GRANT_CACHEKEY_FORMAT, providerName, providerKey, name);
        _logger.LogDebug($"PermissionStore.GetCacheItemAsync: {cacheKey}");
        permissionCached.TryGetValue(cacheKey, out string value);

        if (value != null)
        {
            _logger.LogDebug($"Found in the cache: {cacheKey}");
            return (cacheKey, Convert.ToBoolean(value));
        }

        _logger.LogDebug($"Not found in the cache: {cacheKey}");
        return await SetCacheItemsAsync(providerName, providerKey, name);
    }



    protected virtual async Task<(string Key, bool IsGranted)> SetCacheItemsAsync(string providerName, string providerKey, string currentName)
    {
        var permissions = _permissionDefinitionManager.GetPermissions();
        _logger.LogDebug($"Getting all granted permissions from the repository for this provider name,key: {providerName},{providerKey}");
        var grantedPermissionsHashSet = new HashSet<string>((await _permissionGrantStore.GetListAsync(providerName, providerKey)).Select(p => p.Name));
        _logger.LogDebug($"Setting the cache items. Count: {permissions.Count}");
        bool currentResult = false;
        foreach (var permission in permissions)
        {
            bool isGranted = grantedPermissionsHashSet.Contains(permission.Name);
            permissionCached.TryAdd(string.Format(CacheKeyFormatConstantValue.PERMISSION_GRANT_CACHEKEY_FORMAT, providerName, providerKey, permission.Name), isGranted.ToString());
            if (permission.Name == currentName)
            {
                currentResult = isGranted;
            }
        }

        _logger.LogDebug($"Finished setting the cache items. Count: {permissions.Count}");
        return (string.Format(CacheKeyFormatConstantValue.PERMISSION_GRANT_CACHEKEY_FORMAT, providerName, providerKey, currentName), currentResult);
    }



    protected virtual async Task<List<(string Key, bool IsGranted)>> GetCacheItemsAsync(string[] names,
        string providerName, string providerKey)
    {
        var cacheKeys = names.Select(x => string.Format(CacheKeyFormatConstantValue.PERMISSION_GRANT_CACHEKEY_FORMAT, providerName, providerKey, x)).ToList();

        _logger.LogDebug($"PermissionStore.GetCacheItemAsync: {string.Join(",", cacheKeys)}");

        List<(string key, string value)> getCacheItemTasks = [];

        foreach (string cacheKey in cacheKeys)
        {
            if (permissionCached.TryGetValue(cacheKey, out string value))
            {
                getCacheItemTasks.Add((cacheKey, value));
            }
            else
            {
                getCacheItemTasks.Add((cacheKey, null));
            }
        }

        if (getCacheItemTasks.All(x => x.value != null))
        {
            _logger.LogDebug($"Found in the cache: {string.Join(",", cacheKeys)}");
            return Array.ConvertAll(getCacheItemTasks.ToArray(), i => (i.key, Convert.ToBoolean(i.value))).ToList();
        }

        List<string> notCacheKeys = getCacheItemTasks.Where(x => x.value is null).Select(x => x.key).ToList();

        _logger.LogDebug($"Not found in the cache: {string.Join(",", notCacheKeys)}");

        return await SetCacheItemsAsync(providerName, providerKey, notCacheKeys);
    }


    protected virtual async Task<List<(string Key, bool IsGranted)>> SetCacheItemsAsync(string providerName, string providerKey, List<string> notCacheKeys)
    {
        List<PermissionDefinition> permissions = _permissionDefinitionManager.GetPermissions()
            .Where(x => notCacheKeys.Any(k => GetPermissionInfoFormCacheKey(k).Name == x.Name)).ToList();

        _logger.LogDebug($"Getting not cache granted permissions from the repository for this provider name,key: {providerName},{providerKey}");

        var grantedPermissionsHashSet = new HashSet<string>((await _permissionGrantStore.GetListAsync(notCacheKeys.Select(k => GetPermissionInfoFormCacheKey(k).Name).ToArray(), providerName, providerKey)).Select(p => p.Name));

        _logger.LogDebug($"Setting the cache items. Count: {permissions.Count}");

        List<(string Key, bool IsGranted)> cacheItems = [];

        foreach (PermissionDefinition permission in permissions)
        {
            bool isGranted = grantedPermissionsHashSet.Contains(permission.Name);
            cacheItems.Add((string.Format(CacheKeyFormatConstantValue.PERMISSION_GRANT_CACHEKEY_FORMAT, providerName, providerKey, permission.Name), isGranted));
        }

        foreach ((string key, bool isGranted) in cacheItems)
        {
            permissionCached.TryAdd(key, isGranted.ToString());
        }

        _logger.LogDebug($"Finished setting the cache items. Count: {permissions.Count}");

        return cacheItems;
    }



    protected virtual (string ProviderName, string ProviderKey, string Name) GetPermissionInfoFormCacheKey(
            string key)
    {
        string pattern = @"^pn:(?<providerName>.+),pk:(?<providerKey>.+),n:(?<name>.+)$";

        Match match = Regex.Match(key, pattern, RegexOptions.IgnoreCase);

        string providerName = match.Groups["providerName"].Value;
        string providerKey = match.Groups["providerKey"].Value;
        string name = match.Groups["name"].Value;

        return (providerName, providerKey, name);
    }

    #endregion
}
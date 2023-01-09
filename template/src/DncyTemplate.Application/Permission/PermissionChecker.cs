using Dncy.Permission;
using Dncy.Permission.Models;
using System.Security.Claims;

namespace DncyTemplate.Application.Permission;

public class PermissionChecker : IPermissionChecker
{
    protected readonly IPermissionDefinitionManager _permissionDefinitionManager;

    protected readonly IEnumerable<IPermissionValueProvider> _permissionValueProviders;

    public PermissionChecker(IPermissionDefinitionManager permissionDefinitionManager,
        IEnumerable<IPermissionValueProvider> permissionValueProviders)
    {
        _permissionDefinitionManager = permissionDefinitionManager;
        _permissionValueProviders = permissionValueProviders;
    }


    /// <inheritdoc />
    public virtual async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        var permissionDefinition = _permissionDefinitionManager.Get(name);

        if (!permissionDefinition.IsEnabled)
        {
            return false;
        }

        foreach (var permissionValueProvider in _permissionValueProviders)
        {
            if (permissionDefinition.AllowedProviders.Any() &&
                !permissionDefinition.AllowedProviders.Contains(permissionValueProvider.Name))
            {
                continue;
            }
            var result = await permissionValueProvider.CheckAsync(claimsPrincipal, permissionDefinition);
            if (result == PermissionGrantResult.Granted)
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc />
    public virtual async Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string[] names)
    {
        MultiplePermissionGrantResult result = new MultiplePermissionGrantResult();

        names ??= Array.Empty<string>();

        List<PermissionDefinition> permissionDefinitions = new List<PermissionDefinition>();

        foreach (string name in names)
        {
            var permission = _permissionDefinitionManager.Get(name);
            if (permission == null)
            {
                result.Result.Add(name, PermissionGrantResult.Undefined);
                continue;
            }

            if (permission.IsEnabled)
            {
                permissionDefinitions.Add(permission);
            }
        }

        foreach (var permissionValueProvider in _permissionValueProviders)
        {
            var pf = permissionDefinitions.Where(x => !x.AllowedProviders.Any() || x.AllowedProviders.Contains(permissionValueProvider.Name)).ToList();

            var multipleResult = await permissionValueProvider.CheckAsync(claimsPrincipal, pf);

            foreach (var grantResult in multipleResult.Result)
            {
                if (result.Result.ContainsKey(grantResult.Key))
                {
                    if (result.Result[grantResult.Key] == PermissionGrantResult.Granted || result.Result[grantResult.Key] == PermissionGrantResult.Undefined)
                    {
                        continue;
                    }
                    result.Result[grantResult.Key] = grantResult.Value;
                }
                else
                {
                    result.Result[grantResult.Key] = grantResult.Value;
                }
            }
        }

        return result;
    }

}
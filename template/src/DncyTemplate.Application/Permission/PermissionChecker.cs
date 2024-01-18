using Dotnetydd.Permission.Checker;
using Dotnetydd.Permission.Definition;
using Dotnetydd.Permission.Models;
using Dotnetydd.Permission.ValueProvider;
using System.Security.Claims;
using DncyTemplate.Constants;

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
            return await Task.FromResult(false);
        }

        var userPermissions = claimsPrincipal.FindFirst(UserClaimConstantValue.CLAIM_PERMISSION);
        if (userPermissions == null)
        {
            return await Task.FromResult(false);
        }

        var userPermissionsArr = userPermissions.Value.Split('|');
        if (userPermissionsArr.Contains(name))
        {
            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }

    /// <inheritdoc />
    public virtual async Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string[] names)
    {
        MultiplePermissionGrantResult result = new MultiplePermissionGrantResult();

        names ??= [];

        var userPermissions = claimsPrincipal.FindFirst(UserClaimConstantValue.CLAIM_PERMISSION);
        var userPermissionsArr = userPermissions?.Value.Split('|');
        foreach (string name in names)
        {
            var permission = _permissionDefinitionManager.Get(name);
            if (permission == null)
            {
                result.Result.Add(name, PermissionGrantResult.Undefined);
                continue;
            }

            if (userPermissions == null)
            {
                result.Result.Add(name, PermissionGrantResult.Prohibited);
                continue;
            }

            if (userPermissionsArr.Contains(name))
            {
                result.Result.Add(name, PermissionGrantResult.Granted);
                continue;
            }
        }
        return await Task.FromResult(result);
    }

}
using Dncy.Permission;
using DncyTemplate.Application.Permission.Models;

namespace DncyTemplate.Application.Permission;

[Injectable(InjectLifeTime.Scoped, typeof(IPermissionAppService))]
[AutoResolveDependency]
public partial class PermissionAppService : IPermissionAppService
{
    [AutoInject]
    private readonly IPermissionManager _permissionManager;

    [AutoInject]
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;


    public async Task<List<PermissionGroupDto>> GetPermissionsAsync(string providerName,string providerValue)
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
            var grantResult= await _permissionManager.IsGrantedAsync(grantModel.Select(x => x.Name).ToArray(), providerName, providerValue);
            foreach (var permission in item.GetPermissionsWithChildren())
            {
                _ = grantResult.Result.TryGetValue(permission.Name, out var isGranted);
                var permiss = new PermissionDto
                {
                    Name = permission.Name,
                    DisplayName = permission.DisplayName,
                    ParentName = permission.Parent?.Name!,
                    IsGrant = isGranted==Dncy.Permission.Models.PermissionGrantResult.Granted,
                    AllowProviders = permission.AllowedProviders.ToArray(),
                };
                group.Permissions.Add(permiss);
            }
            res.Add(group);
        }

        return res;
    }
}
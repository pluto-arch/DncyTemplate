using Dncy.Permission;

namespace DncyTemplate.Application.Permission;

[Injectable(InjectLifeTime.Scoped, typeof(IPermissionAppService))]
[AutoResolveDependency]
public partial class PermissionAppService : IPermissionAppService
{
    [AutoInject]
    private readonly IPermissionManager _permissionManager;

    [AutoInject]
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;


    public List<dynamic> GetPermissions()
    {
        var res = new List<dynamic>();
        var groups = _permissionDefinitionManager.GetGroups();
        foreach (var item in groups)
        {
            var group = new
            {
                groupName = item.Name,
                displayName = item.DisplayName,
                permissions = new List<dynamic>()
            };

            foreach (var permission in item.GetPermissionsWithChildren())
            {
                var permissionGrantModel = new
                {
                    permission.Name,
                    permission.DisplayName,
                    ParentName = permission.Parent?.Name!,
                    permission.AllowedProviders,
                };
                group.permissions.Add(permissionGrantModel);
            }
            res.Add(group);
        }

        return res;
    }
}
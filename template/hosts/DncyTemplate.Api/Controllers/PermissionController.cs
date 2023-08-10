using Dncy.Permission;
using DncyTemplate.Application.Models;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[controller]")]
    [AutoResolveDependency]
    [ApiController]
    public partial class PermissionController : ControllerBase, IResponseWraps
    {
        [AutoInject]
        private readonly IPermissionGrantStore _permissionGrantStore;

        [AutoInject]
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;


        /// <summary>
        ///     获取权限
        /// </summary>
        /// <param name="providerName">提供者名称 eg. Role</param>
        /// <param name="providerKey">提供者值 eg. admin</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResultDto> GetAsync(string providerName, string providerKey)
        {
            await Task.Yield();
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
                    if (permission.IsEnabled && ( !permission.AllowedProviders.Any() ||
                                                 permission.AllowedProviders.Contains(providerName) ))
                    {

                        if (permission.AllowedProviders.Any() && !permission.AllowedProviders.Contains(providerName))
                        {
                            throw new ApplicationException(
                                $"The permission named {permission.Name} has not compatible with the provider named {providerName}");
                        }

                        if (!permission.IsEnabled)
                        {
                            throw new ApplicationException($"The permission named {permission.Name} is disabled");
                        }

                        var permissionGrant = await _permissionGrantStore.GetAsync(permission.Name, providerName, providerKey);
                        var permissionGrantModel = new
                        {
                            permission.Name,
                            permission.DisplayName,
                            ParentName = permission.Parent,
                            permission.AllowedProviders,
                            IsGranted = permissionGrant != null,
                        };
                        group.permissions.Add(permissionGrantModel);

                    }
                }
                res.Add(group);
            }

            return this.Success(res);

        }


        /// <summary>
        ///     获取权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("getList")]
        public async Task<ResultDto> GetListAsync()
        {
            await Task.Yield();
            return this.Success(_permissionDefinitionManager.GetGroups());
        }


    }
}

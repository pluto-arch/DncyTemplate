using DncyTemplate.Application.Models;
using DncyTemplate.Application.Permission.Models;
using Dotnetydd.Permission.Definition;
using Dotnetydd.Permission.Models;
using Dotnetydd.Permission.PermissionGrant;
using Microsoft.AspNetCore.Authorization;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[controller]")]
    [AutoResolveDependency]
    [ApiController]
    [Authorize]
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
            var res = new List<dynamic>();
            var groups = _permissionDefinitionManager.GetGroups();

            var providerPermission = await _permissionGrantStore.GetListAsync(providerName, providerKey);

            foreach (var item in groups)
            {
                var group = new PermissionGroupDto
                {
                    Name = item.Name,
                    DisplayName = item.DisplayName,
                    Permissions = []
                };

                foreach (var permission in item.Permissions)
                {
                    if (permission.IsEnabled && (!permission.AllowedProviders.Any() ||
                                                 permission.AllowedProviders.Contains(providerName)))
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

                        var permissionGrantModel = GetProviderPermissionInfo(permission, providerPermission);
                        group.Permissions.AddRange(permissionGrantModel);
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


        [NonAction]
        private List<PermissionDto> GetProviderPermissionInfo(PermissionDefinition permission,
            IEnumerable<IPermissionGrant> grantPermissionList)
        {
            var res = new List<PermissionDto>();

            var permissionGrant = grantPermissionList.FirstOrDefault(x => x.Name == permission.Name);
            var permissionGrantModel = new PermissionDto
            {
                Name = permission.Name,
                DisplayName = permission.DisplayName,
                ParentName = permission.Parent,
                IsGrant = permissionGrant != null,
                AllowProviders = [.. permission.AllowedProviders],
                Children = []
            };

            if (permission.Children.Any())
            {
                foreach (var child in permission.Children)
                {
                    permissionGrantModel.Children.AddRange(GetProviderPermissionInfo(child, grantPermissionList));
                }
            }

            res.Add(permissionGrantModel);
            return res;
        }
    }
}
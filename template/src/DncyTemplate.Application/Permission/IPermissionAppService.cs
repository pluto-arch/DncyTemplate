using DncyTemplate.Application.Permission.Models;

namespace DncyTemplate.Application.Permission;

public interface IPermissionAppService
{
    /// <summary>
    /// 分页筛选获取权限树结构（树形结构数据）
    /// </summary>
    /// <returns></returns>
    Task<List<PermissionGroupDto>> GetPermissionsAsync(string providerName,string providerValue);

    Task GrantAsync(string[] permissions, string providerName, string providerValue);
}
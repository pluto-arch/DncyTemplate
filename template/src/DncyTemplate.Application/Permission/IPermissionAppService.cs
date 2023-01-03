using DncyTemplate.Application.Permission.Models;
using DncyTemplate.Domain.Collections;

namespace DncyTemplate.Application.Permission;

public interface IPermissionAppService
{
    /// <summary>
    /// 分页筛选获取权限树结构（树形结构数据）
    /// </summary>
    /// <returns></returns>
    List<dynamic> GetPermissions();
}
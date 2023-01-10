namespace DncyTemplate.Mvc.Models.Role;

public class RoleForEditViewModel
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public RoleDto Role { get; set; }

    /// <summary>
    /// 所有权限列表集合
    /// </summary>
    public List<PermissionTreeViewModel> Permissions { get; set; }
}
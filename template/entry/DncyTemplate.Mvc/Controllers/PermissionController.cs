using Dncy.Permission;
using DncyTemplate.Application.Permission;
using DncyTemplate.Mvc.Models.Permission;

namespace DncyTemplate.Mvc.Controllers;


[AutoResolveDependency]
public partial class PermissionController : Controller
{
    [AutoInject]
    private readonly IPermissionAppService _permissionAppService;

    /// <summary>
    /// 权限列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> PermissionsAsync(string providerName,string providerValue)
    {
        var permissions = await _permissionAppService.GetPermissionsAsync(providerName,providerValue);
        return Json(permissions);
    }

    [HttpPost]
    public async Task<IActionResult> GrantAsync([FromQuery]string providerName, [FromQuery] string providerValue, [FromBody] PermissionSelectedViewModel permissions)
    {
        await _permissionAppService.GrantAsync(permissions.Permissions.ToArray(), providerName, providerValue);
        return Json(new {success=true,msg="操作成功"});
    }
}
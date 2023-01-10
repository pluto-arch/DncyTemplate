using DncyTemplate.Application.Permission;

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
}
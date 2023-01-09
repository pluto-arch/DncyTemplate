using DncyTemplate.Application.Permission;

namespace DncyTemplate.Mvc.Controllers;


[AutoResolveDependency]
public partial class PermissionController : Controller
{
    [AutoInject]
    private readonly IPermissionAppService _permissionAppService;

    // GET
    public IActionResult Index()
    {
        return View();
    }


    /// <summary>
    /// 权限列表
    /// </summary>
    /// <returns></returns>
    public JsonResult GetPermissionList()
    {
        var permissions = _permissionAppService.GetPermissions();
        return Json(permissions);
    }
}
#if Tenant

using Dncy.MultiTenancy.ConnectionStrings;
using DncyTemplate.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace DncyTemplate.Mvc.Controllers;

/// <summary>
/// 租户控制器
/// </summary>
[AutoResolveDependency]
[Authorize(Roles = "SA")]
public partial class TenantController : Controller
{
    [AutoInject]
    protected readonly IOptions<TenantConfigurationOptions> _tenantConfigurationOptions;

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult List()
    {
        var tenants = _tenantConfigurationOptions.Value.Tenants;
        var tenantVm = new List<TenantViewModel>();
        foreach (var item in tenants)
        {
            tenantVm.Add(new TenantViewModel
            {
                TenantId = item.TenantId,
                TenantName = item.TenantName,
                IsAvaliable = item.IsAvaliable,
            });
        }
        return Json(new
        {
            code = 200,
            count = tenants.Count(),
            msg = "操作成功",
            data = tenantVm
        });
    }

    [HttpGet]
    public IActionResult Detail(string tenantId)
    {
        var tenant = _tenantConfigurationOptions.Value.Tenants.FirstOrDefault(x => x.TenantId == tenantId);
        if (tenant == null)
        {
            return NotFound();
        }
        return Json(new
        {
            code = 200,
            msg = "操作成功",
            data = new TenantViewModel
            {
                TenantId = tenant.TenantId,
                TenantName = tenant.TenantName,
                IsAvaliable = tenant.IsAvaliable,
                ConnectionStrings = tenant.ConnectionStrings,
            }
        });
    }
}
#endif
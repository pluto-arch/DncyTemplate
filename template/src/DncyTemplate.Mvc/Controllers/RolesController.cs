using System.Net;
using DncyTemplate.Application.Permission;
using DncyTemplate.Infra.Utils;
using DncyTemplate.Mvc.Models.Account;
using DncyTemplate.Mvc.Models.Role;
using Dotnetydd.Tools.Extension;

namespace DncyTemplate.Mvc.Controllers;


[AutoResolveDependency]
public partial class RolesController : Controller
{

    [AutoInject]
    private readonly IPermissionAppService _permissionAppService;

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> PermissionGrant(string roleName)
    {
        if (roleName.IsNullOrEmpty())
        {
            return RedirectToAction("Error", "Error", new { code = HttpStatusCode.NotFound, error = "角色不存在" });
        }
        var role = roleName.ToLower();
        if (role != "sa" && role != "admin" && role != "member")
        {
            return RedirectToAction("Error", "Error", new { code = HttpStatusCode.NotFound, error = "角色不存在" });
        }
        var permissions = await _permissionAppService.GetPermissionsAsync("role", roleName);
        var permissionsVm = new List<PermissionTreeViewModel>();
        foreach (var group in permissions)
        {
            var g = new PermissionTreeViewModel
            {
                Id = group.Name,
                Title = group.DisplayName,
                Children = [],
                CheckArr = group.Permissions.Any(x => x.IsGrant) ? "1" : "0",
            };
            group.Permissions = [.. group.Permissions.OrderBy(x => x.Name)];
            foreach (var item in group.Permissions)
            {
                if (item.ParentName.IsNullOrEmpty())
                {
                    g.Children.Add(new PermissionTreeViewModel
                    {
                        Id = item.Name,
                        ParentId = g.Id,
                        Title = item.DisplayName,
                        IsAssigned = item.IsGrant,
                    });
                }
                else
                {
                    var up = g.Children.FirstOrDefault(x => x.Id == item.ParentName);
                    up.Children.Add(new PermissionTreeViewModel
                    {
                        Id = item.Name,
                        ParentId = up.Id,
                        Title = item.DisplayName,
                        IsAssigned = item.IsGrant,
                    });
                }
            }
            permissionsVm.Add(g);
        }

        ViewData["roleName"] = roleName;

        return View(new RolePermissionGrantViewModel
        {
            Code = 0,
            Msg = "操作成功",
            Data = permissionsVm
        });
    }


    [HttpGet]
    public IActionResult List()
    {
        var roles = new List<RoleViewModel>
        {
            new()
            {
                Id = SnowFlakeId.Generator.GetUniqueShortId(),
                RoleName = RoleEnum.SA.ToString(),
                DisplayName=RoleEnum.SA.GetDescription(),
                CreateTime = DateTime.Now
            },
            new()
            {
                Id = SnowFlakeId.Generator.GetUniqueShortId(),
                RoleName = RoleEnum.Admin.ToString(),
                DisplayName=RoleEnum.Admin.GetDescription(),
                CreateTime = DateTime.Now
            },
            new()
            {
                Id = SnowFlakeId.Generator.GetUniqueShortId(),
                RoleName = RoleEnum.Member.ToString(),
                DisplayName=RoleEnum.Member.GetDescription(),
                CreateTime = DateTime.Now
            }
        };
        return Json(new
        {
            code = 200,
            count = roles.Count,
            msg = "操作成功",
            data = roles
        });
    }

}
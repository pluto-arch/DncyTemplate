using DncyTemplate.Application.Models.Application.Navigation;
using DncyTemplate.Application.Permission;
using Microsoft.Extensions.Localization;
using System.Collections.Immutable;

namespace DncyTemplate.Mvc.Views.Shared.Components.SideBarMenu;

public class SideBarMenuViewComponent : ViewComponent
{
    public static List<MenuItemModel> Menus = new();
    private static object _instance = new object();

    public async Task<IViewComponentResult> InvokeAsync()
    {
        await Task.Yield();
        if (Menus.Any())
        {
            return View(new SideBarMenuViewModel
            {
                Items = Menus.ToImmutableList()
            });
        }

        lock (_instance)
        {
            if (!Menus.Any())
            {
                initMenu();
            }
        }
        return View(new SideBarMenuViewModel
        {
            Items = Menus.ToImmutableList()
        });
    }


    void initMenu()
    {
        // dashboard
        Menus.Add(new MenuItemModel
        {
            Name = "app.menu.dashboard.hostconsole",
            Icon = "layui-icon-console",
            DisplayName = "Dashboard",
            Url = "/dashboard/hostconsole",
            IsEnabled = true,
            IsVisible = true,
            Permission = MenuPermission.SkipCheckPermission(),
        });


        Menus.Add(new MenuItemModel
        {
            Name = "app.menu.device",
            Icon = "layui-icon-read",
            DisplayName = "Devices",
            IsEnabled = true,
            IsVisible = true,
            Items = new List<MenuItemModel>
            {
                new MenuItemModel
                {
                    Name = "app.menu.device.products",
                    Icon = "layui-icon-console",
                    DisplayName = "Product List",
                    Url = "/product",
                    IsEnabled = true,
                    IsVisible = true,
                    Permission = new MenuPermission(new[] { ProductPermission.Product.Default }),
                },
                new MenuItemModel
                {
                    Name = "app.menu.device.devices",
                    Icon = "layui-icon-console",
                    DisplayName = "设备管理",
                    Url = "/device",
                    IsEnabled = true,
                    IsVisible = true,
                    Permission = new MenuPermission(new[] { DevicesPermission.Devices.Default }),
                }
            }
        });


        Menus.Add(new MenuItemModel
        {
            Name = "app.menu.accescontrol",
            Icon = "layui-icon-read",
            DisplayName = "访问控制",
            IsEnabled = true,
            IsVisible = true,
            Items = new List<MenuItemModel>
            {
#if Tenant
                new MenuItemModel
                {
                    Name = "app.menu.accescontrol.tenants",
                    Icon = "layui-icon-console",
                    DisplayName = "租户列表",
                    Url = "/tenant",
                    IsEnabled = true,
                    IsVisible = true,
                    Permission=new MenuPermission(true)
                },
#endif
                new MenuItemModel
                {
                    Name = "app.menu.accescontrol.roles",
                    Icon = "layui-icon-console",
                    DisplayName = "角色列表",
                    Url = "/roles",
                    IsEnabled = true,
                    IsVisible = true,
                    Permission=new MenuPermission(true)
                }
            }
        });

    }
}
using System.Collections.Immutable;
using DncyTemplate.Application.Constants;
using DncyTemplate.Application.Models.Application.Navigation;
using DncyTemplate.Application.Permission;
using Microsoft.Extensions.Localization;

namespace DncyTemplate.Mvc.Views.Shared.Components.SideBarMenu;

public class SideBarMenuViewComponent : ViewComponent
{
    public static List<MenuItemModel> Menus = new();
    private readonly IStringLocalizer<SharedResource> _localizer;

    public SideBarMenuViewComponent(IStringLocalizer<SharedResource> localizer)
    {
        _localizer = localizer;
    }


    public async Task<IViewComponentResult> InvokeAsync()
    {
        await Task.Yield();
        if (!Menus.Any())
        {
            initMenu();
        }

        var model = new SideBarMenuViewModel
        {
            Items = Menus.ToImmutableList()
        };

        return View(model);
    }


    void initMenu()
    {
        // dashboard
        Menus.Add(new MenuItemModel
        {
            Name = "app.menu.dashboard.hostconsole",
            Icon = "layui-icon-console",
            DisplayName = "控制台",
            Url = "/dashboard/hostconsole",
            IsEnabled = true,
            IsVisible = true,
            Permission = MenuPermission.SkipCheckPermission(),
        });


        Menus.Add(new MenuItemModel
        {
            Name = "app.menu.device",
            Icon = "layui-icon-read",
            DisplayName = "设备管理",
            IsEnabled = true,
            IsVisible = true,
            Permission = new MenuPermission(new[] { ProductPermission.Product.Default,DevicesPermission.Devices.Default}),
            Items = new List<MenuItemModel>
            {
                new MenuItemModel
                {
                    Name = "app.menu.device.products",
                    Icon = "layui-icon-console",
                    DisplayName = "产品管理",
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
    }
}
using DncyTemplate.Application.AppServices.System;

namespace DncyTemplate.Mvc.ViewComponents.Layout
{
    [ViewComponent(Name = "SideMenu")]
    public class SideMenu : ViewComponent
    {
        private readonly ISystemAppService _appService;

        public SideMenu(ISystemAppService appService)
        {
            _appService = appService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menus = await _appService.GetSysMenus();
            return View(menus);
        }
    }
}
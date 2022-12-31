namespace DncyTemplate.Mvc.Views.Shared.Components.SideBarMenu;

public class SideBarMenuViewComponent:ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        await Task.Yield();
        return View();
    }
}
namespace DncyTemplate.Mvc.Views.Shared.Components.RightNavbarUserArea;

public class RightNavbarUserAreaViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        await Task.Yield();
        return View();
    }
}
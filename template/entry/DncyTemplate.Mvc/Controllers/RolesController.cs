namespace DncyTemplate.Mvc.Controllers;


[AutoResolveDependency]
public partial class RolesController: Controller
{
    public IActionResult Index() { return View(); }
}
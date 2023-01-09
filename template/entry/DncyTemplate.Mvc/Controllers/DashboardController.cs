using Microsoft.AspNetCore.Authorization;

namespace DncyTemplate.Mvc.Controllers;

[Authorize]
public class DashboardController : Controller
{
    // GET
    public IActionResult HostConsole()
    {
        return View();
    }
}
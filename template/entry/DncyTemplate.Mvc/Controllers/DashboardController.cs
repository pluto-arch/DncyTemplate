using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
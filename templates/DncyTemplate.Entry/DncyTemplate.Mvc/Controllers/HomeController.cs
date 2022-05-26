using System.Diagnostics;

using DncyTemplate.Mvc.Models;

using Microsoft.AspNetCore.Mvc;

namespace DncyTemplate.Mvc.Controllers
{
    [AutoResolveDependency]
    public partial class HomeController : Controller
    {
        [AutoInject]
        private readonly ILogger<HomeController> _logger;


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogWarning("has an error");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
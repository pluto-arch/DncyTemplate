using DncyTemplate.Mvc.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace DncyTemplate.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }


        public IActionResult Index()
        {
            ViewBag.Demo = _stringLocalizer[SharedResource.Hello];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SwitchLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogWarning("has an error");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
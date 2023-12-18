using CommunityToolkit.Diagnostics;
using DncyTemplate.Application.Command.Product;
using DncyTemplate.Mvc.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;


namespace DncyTemplate.Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _stringLocalizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> stringLocalizer)
        {
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }


        [EnableRateLimiting(policyName:"home.RateLimit_action")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Users()
        {
            _logger.LogInformation("接收到请求");
            return Ok("Hello");
        }



        // [Authorize(ProductPermission.Product.Create)]
        // public async Task<IActionResult> Generate([FromServices] IHostEnvironment env)
        // {
        //     var models = await _repository.AsNoTracking().Select(x => new ProductListItemDto(x.Id, x.Name, x.CreationTime.DateTime)).ToPagedListAsync(1, 20);
        //     ViewData["data"] = models;
        //     ViewData["Host"] = Request.IsHttps ? $"https://{Request.Host}" : $"http://{Request.Host}";
        //     IViewEngine viewEngine = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
        //     ViewEngineResult viewResult = viewEngine?.FindView(ControllerContext, "Templates/ProductListTemplate", true);
        //     if (viewResult?.Success == false)
        //     {
        //         return Ok("error");
        //     }
        //
        //     await using var writer = new StringWriter();
        //     if (viewResult != null)
        //     {
        //         var viewContext = new ViewContext(
        //             ControllerContext,
        //             viewResult.View,
        //             ViewData,
        //             TempData,
        //             writer,
        //             new HtmlHelperOptions()
        //         );
        //         await viewResult.View.RenderAsync(viewContext);
        //     }
        //
        //     var html = writer.GetStringBuilder().ToString();
        //     var culture = Request.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture;
        //     var path = Path.Combine(env.ContentRootPath, "wwwroot", "htmls", $"product_{culture?.Name}.html");
        //     await System.IO.File.WriteAllTextAsync(path, html);
        //     return RedirectToAction(nameof(Product));
        // }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult TestLocalization()
        {
            var text = _stringLocalizer["HelloWorld"];
            return Ok(text);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SwitchLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

#if Tenant
        [HttpPost]
        public IActionResult SwitchTenant([FromForm] string tenantId, string returnUrl)
        {
            Response.Cookies.Append(AppConstant.TENANT_KEY, tenantId, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(1) });
            return LocalRedirect(returnUrl);
        }
#endif
    }
}
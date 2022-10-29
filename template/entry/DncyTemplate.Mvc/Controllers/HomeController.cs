﻿using DncyTemplate.Application.Models.Product;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Repository;
using DncyTemplate.Infra.EntityFrameworkCore.Extension;
using DncyTemplate.Mvc.Constants;

using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;


namespace DncyTemplate.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;
        private readonly IRepository<Product> _repository;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<SharedResource> stringLocalizer, IRepository<Product> repository)
        {
            _logger = logger;
            _stringLocalizer = stringLocalizer;
            _repository = repository;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Product()
        {
            var models = await _repository.AsNoTracking().Select(x => new ProductListItemDto(x.Id, x.Name, x.CreationTime.DateTime)).ToPagedListAsync(1, 200);
            return View(models);
        }


        public async Task<IActionResult> Generate([FromServices] IHostEnvironment env)
        {
            var models = await _repository.AsNoTracking().Select(x => new ProductListItemDto(x.Id, x.Name, x.CreationTime.DateTime)).ToPagedListAsync(1, 20);
            ViewData["data"] = models;
            ViewData["Host"] = Request.IsHttps ? $"https://{Request.Host}" : $"http://{Request.Host}";
            IViewEngine viewEngine = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            ViewEngineResult viewResult = viewEngine?.FindView(ControllerContext, "Templates/ProductListTemplate", true);
            if (viewResult?.Success == false)
            {
                return Ok("error");
            }

            await using var writer = new StringWriter();
            if (viewResult != null)
            {
                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );
                await viewResult.View.RenderAsync(viewContext);
            }

            var html = writer.GetStringBuilder().ToString();
            var culture = Request.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture;
            var path = Path.Combine(env.ContentRootPath, "wwwroot", "htmls", $"product_{culture?.Name}.html");
            await System.IO.File.WriteAllTextAsync(path, html);
            return RedirectToAction(nameof(Product));
        }



        [HttpGet]
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
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public IActionResult SwitchTenant([FromForm] string tenantId, string returnUrl)
        {
            Response.Cookies.Append(AppConstant.TENANT_KEY, tenantId, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(1) });
            return LocalRedirect(returnUrl);
        }
    }
}
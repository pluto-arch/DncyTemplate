using DncyTemplate.Application.AppServices.Product;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Domain.Collections;
using Microsoft.AspNetCore.Authorization;

namespace DncyTemplate.Mvc.Controllers
{
    [Authorize]
    [AutoResolveDependency]
    public partial class ProductController : Controller
    {
        [AutoInject]
        private readonly ILogger<HomeController> _logger;
        [AutoInject]
        private readonly IProductAppService _productAppService;

        public async Task<IActionResult> Index([FromQuery] ProductPagedRequest request)
        {
            var pageData = await _productAppService.GetListAsync(request);
            ViewBag.Keywords = request.Keyword;
            return View(pageData);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateAsync([FromForm]ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _productAppService.CreateAsync(request);
            return RedirectToAction("Index");
        }

    }
}

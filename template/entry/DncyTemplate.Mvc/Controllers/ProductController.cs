using DncyTemplate.Application.AppServices.Product;
using DncyTemplate.Application.Models.Product;
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
            return View(pageData);
        }
    }
}

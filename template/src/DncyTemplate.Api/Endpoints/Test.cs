using DncyTemplate.Api.Models;
using DncyTemplate.Application.Command.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [AutoResolveDependency]
    [ApiController]
    [AllowAnonymous]
    public partial class Test : EndPointBase
    {
        [AutoInject]
        private readonly IStringLocalizer<SharedResources> _sharedres;


        [HttpGet]
        public IActionResult TestLocalize(string key)
        {
            var text = _sharedres[key];
            if (key.Length > 3)
            {
                throw new InvalidOperationException("1111111");
            }
            return Ok(text);
        }



        [HttpPost]
        public IActionResult Post(DemoModel request)
        {
            return Ok("ok");
        }

        [HttpPost]
        public IActionResult CreateProduct(CreateProductCommand request)
        {
            return Ok("ok");
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase,IApiResultWapper
    {
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public HomeController(IStringLocalizer<SharedResource> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet]
        public ApiResult TestLocalize()
        {
            var text = _stringLocalizer[SharedResource.Hello];
            return this.Success<string>(text);
        }
    }
}

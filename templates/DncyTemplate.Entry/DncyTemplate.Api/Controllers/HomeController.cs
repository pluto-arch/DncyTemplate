using Dncy.MultiTenancy;
using Microsoft.Extensions.Localization;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[controller]")]
    [AutoResolveDependency]
    [ApiController]
    public partial class HomeController : ControllerBase, IApiResultWapper
    {
        [AutoInject]
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;

        [HttpGet]
        public ApiResult TestLocalize()
        {
            var text = _stringLocalizer[SharedResource.Hello];
            return this.Success<string>(text);
        }
    }
}

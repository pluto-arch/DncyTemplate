using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[controller]")]
    [AutoResolveDependency]
    [ApiController]
    [AllowAnonymous]
    public partial class HomeController : ControllerBase, IApiResultWapper
    {
        [AutoInject]
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;

        [HttpGet]
        public ApiResult TestLocalize([EmailAddress] string name)
        {
            var text = _stringLocalizer[SharedResource.Hello];
            return this.Success<string>(text);
        }

        /// <summary>
        /// 限流测试
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("/rate_limit")]
        [EnableRateLimiting("home.RateLimit_action")]
        public ApiResult RateLimit([EmailAddress] string name)
        {
            var text = _stringLocalizer[SharedResource.Hello];
            return this.Success<string>(text);
        }
    }
}

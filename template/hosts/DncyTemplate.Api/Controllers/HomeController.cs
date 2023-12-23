using DncyTemplate.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [AutoResolveDependency]
    [ApiController]
    [AllowAnonymous]
    public partial class HomeController : ControllerBase, IResponseWraps
    {
        [AutoInject]
        private readonly IStringLocalizer<SharedResources> _sharedres;


        [HttpGet]
        public IActionResult TestLocalize(string key)
        {
            var text = _sharedres[key];
            return Ok(text);
        }

        [HttpGet]
        public ResultDto TestActionException(string name)
        {
            ThrowHelper.ThrowArgumentException(nameof(name), "name 不能 为空");
            return this.Success<string>(name);
        }

        /// <summary>
        /// 限流测试
        /// </summary>
        /// <returns></returns>
        [HttpGet("/rate_limit")]
        [EnableRateLimiting("home.RateLimit_Test")]
        public ResultDto RateLimit()
        {
            return this.Success<string>("ok");
        }
    }

}


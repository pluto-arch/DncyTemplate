using DncyTemplate.Application.Models;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Infra.Repository;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Infra.EntityFrameworkCore.Repository;
using DncyTemplate.Infra.Utils;
using DncyTemplate.Uow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [AutoResolveDependency]
    [ApiController]
    [AllowAnonymous]
    public partial class HomeController : ControllerBase, IResponseWraps
    {
        [AutoInject]
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;

        [AutoInject]
        private readonly IUnitOfWork<DncyTemplateDbContext> _efUow;

        [AutoInject]
        private readonly IHttpClientFactory _httpClientFactory;

        [AutoInject]
        private readonly ILogger<HomeController> _logger;


        [HttpGet]
        public ResultDto TestLocalize(int name)
        {
            var text = _stringLocalizer[SharedResource.Hello];
            return this.Success<string>(text);
        }
        
        [HttpGet]
        public ResultDto TestActionException(string name)
        {
            ThrowHelper.ThrowArgumentException(nameof(name),"name 不能 为空");
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
        public ResultDto RateLimit([EmailAddress] string name)
        {
            var text = _stringLocalizer[SharedResource.Hello];
            return this.Success<string>(text);
        }
    }

}


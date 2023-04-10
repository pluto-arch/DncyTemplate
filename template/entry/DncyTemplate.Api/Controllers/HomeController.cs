using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Infra.EntityFrameworkCore;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Infra.EntityFrameworkCore.Repository;
using DncyTemplate.Infra.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[controller]")]
    [AutoResolveDependency]
    [ApiController]
    [AllowAnonymous]
    public partial class HomeController : ControllerBase, IResultWraps
    {
        [AutoInject]
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;

        [AutoInject]
        private readonly EfUnitOfWork<DncyTemplateDbContext> _efUow;

        [AutoInject]
        private readonly IHttpClientFactory _httpClientFactory;

        [AutoInject]
        private readonly ILogger<HomeController> _logger;

        [HttpGet]
        public ApiResult TestLocalize([EmailAddress] string name)
        {
            var text = _stringLocalizer[SharedResource.Hello];
            return this.Success<string>(text);
        }


        [HttpGet("/uow")]
        public async Task<ApiResult> RateLimit([FromServices] IEfRepository<Product> _repository, [EmailAddress] string name)
        {
            var a = _efUow.Repository<Product>();
            var aa = a.GetHashCode();
            var dd = _repository.GetHashCode();

            using (_efUow.NewScope())
            {
                await a.InsertAsync(new Product
                {
                    Id = SnowFlakeId.Generator.GetUniqueId(),
                    Name = "change111",
                    Remark = "sdasdasdad",
                    CreationTime = DateTimeOffset.Now,
                });
                await _efUow.CompleteAsync();
            }

            await _repository.InsertAsync(new Product
            {
                Id = SnowFlakeId.Generator.GetUniqueId(),
                Name = "outof change",
                Remark = "sdasdasdad",
                CreationTime = DateTimeOffset.Now,
            });


            var text = _stringLocalizer[SharedResource.Hello];
            return this.Success<string>(text);
        }



        [HttpGet("loc")]
        [AllowAnonymous]
        public async Task<ApiResult> RpcCall()
        {
            var client = _httpClientFactory.CreateClient("mvc");
            _logger.LogInformation("开始请求");
            var response = await client.GetStringAsync("http://localhost:6000/home/users");
            return this.Success(response);
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

using DncyTemplate.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using Bogus;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Infra.Utils;
using DncyTemplate.Uow;
using MapsterMapper;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [AutoResolveDependency]
    [ApiController]
    [AllowAnonymous]
    public partial class HomeController : ControllerBase, IResponseWraps
    {
        [AutoInject]
        private readonly IStringLocalizer<HomeController> _stringLocalizer;

        [AutoInject]
        private readonly IHttpClientFactory _httpClientFactory;

        [AutoInject]
        private readonly ILogger<HomeController> _logger;

        [AutoInject]
        private readonly IMapper _mapper;


        [HttpGet]
        public IActionResult TestLocalize(int name)
        {
            var text = _stringLocalizer[HomeControllerResource.Welcome];
            return Ok(text);
        }

        [HttpGet]
        public ResultDto TestActionException(string name)
        {
            ThrowHelper.ThrowArgumentException(nameof(name), "name 不能 为空");
            var text = _stringLocalizer[HomeControllerResource.Welcome];
            return this.Success<string>(text);
        }

        /// <summary>
        /// 限流测试
        /// </summary>
        /// <returns></returns>
        [HttpGet("/rate_limit")]
        [EnableRateLimiting("home.RateLimit_action")]
        public ResultDto RateLimit()
        {
            var text =  _stringLocalizer[HomeControllerResource.Welcome];
            return this.Success<string>(text);
        }

        [AutoInject]
        private readonly IUnitOfWork<DncyTemplateDbContext> _uow;
        
        
        [HttpGet]
        public async Task<IActionResult> UowThreadSelf()
        {
            var rep = _uow.GetEfRepository<Product>();

            await rep.InsertAsync(new Product
            {
                Id = SnowFlakeId.Generator.GetUniqueId(),
                Name = "AAAAA",
            });


            using (var cop=_uow.BeginNew())
            {
                await rep.InsertAsync(new Product
                {
                    Id = SnowFlakeId.Generator.GetUniqueId(),
                    Name = "BBBBB",
                });
                await cop.CompleteAsync();
            }

            await _uow.CompleteAsync();
            return Ok("success");
        }
    }

}


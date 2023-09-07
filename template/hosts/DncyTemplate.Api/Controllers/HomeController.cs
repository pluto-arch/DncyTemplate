using DncyTemplate.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using Bogus;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Domain.Aggregates.Product;
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
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("/rate_limit")]
        [EnableRateLimiting("home.RateLimit_action")]
        public ResultDto RateLimit()
        {
            var text =  _stringLocalizer[HomeControllerResource.Welcome];
            return this.Success<string>(text);
        }
        
        
        [HttpGet]
        public async Task<IActionResult> UowThreadSelf([FromServices]IUnitOfWork unitOfWork)
        {
            var rep = unitOfWork.GetEfRepository<Product>();
            await rep.InsertAsync(new Product
            {
                Id = "A",
                Name = "主请求作用域",
            });


            var t1 = Task.Run(async () =>
            {
                using (var newuow=unitOfWork.BeginNew())
                {
                    var rep = newuow.GetEfRepository<Product>();
                    await rep.InsertAsync(new Product
                    {
                        Id = "B",
                        Name = "Task1作用域",
                    });
                    await newuow.CompleteAsync();
                }
            });
            
            var t2 = Task.Run(async () =>
            {
                using (var newuow=unitOfWork.BeginNew())
                {
                    var rep = newuow.GetEfRepository<Product>();
                    await rep.InsertAsync(new Product
                    {
                        Id = "C",
                        Name = "Task2作用域",
                    });
                    await newuow.CompleteAsync();
                }
            });

            await Task.WhenAll(t1,t2);
            
            await unitOfWork.CompleteAsync();
            return Ok("success");
        }
        
        
    }

}


using DncyTemplate.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using Bogus;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
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

        // [AutoInject]
        // private readonly IUnitOfWork<DncyTemplateDbContext> _uow1;

        [AutoInject]
        private readonly IUnitOfWork<DncyTemplateDb2Context> _uow2;
        
        
        [HttpGet]
        public async Task<IActionResult> UowThreadSelf([FromServices]IUnitOfWork<DncyTemplateDbContext> uow3)
        {
            // await UowA();
            // await UowB();
            return Ok("success");
        }


        // private async Task UowA()
        // {
        //     var rep = _uow1.GetEfRepository<Product>();
        //     await rep.InsertAsync(new Product
        //     {
        //         Id = "A",
        //         Name = "主请求作用域",
        //     });
        //
        //
        //     var t1 = Task.Run(async () =>
        //     {
        //         using (var newuow=_uow1.BeginNew())
        //         {
        //             await rep.InsertAsync(new Product
        //             {
        //                 Id = "A-T1",
        //                 Name = "Task1作用域",
        //             });
        //             await newuow.CompleteAsync();
        //         }
        //     });
        //     
        //     var t2 = Task.Run(async () =>
        //     {
        //         using (var newuow=_uow1.BeginNew())
        //         {
        //             await rep.InsertAsync(new Product
        //             {
        //                 Id = "A-T2",
        //                 Name = "Task2作用域",
        //             });
        //             await newuow.CompleteAsync();
        //         }
        //     });
        //
        //     await Task.WhenAll(t1,t2);
        //     await _uow1.CompleteAsync();
        // }
        //
        // private async Task UowB()
        // {
        //     var rep = _uow2.GetEfRepository<Product>();
        //     await rep.InsertAsync(new Product
        //     {
        //         Id = "B",
        //         Name = "主请求作用域",
        //     });
        //
        //
        //     var t1 = Task.Run(async () =>
        //     {
        //         using (var newuow=_uow2.BeginNew())
        //         {
        //             await rep.InsertAsync(new Product
        //             {
        //                 Id = "B-T1",
        //                 Name = "Task1作用域",
        //             });
        //             await newuow.CompleteAsync();
        //         }
        //     });
        //     
        //     var t2 = Task.Run(async () =>
        //     {
        //         using (var newuow=_uow2.BeginNew())
        //         {
        //             await rep.InsertAsync(new Product
        //             {
        //                 Id = "B-T2",
        //                 Name = "Task2作用域",
        //             });
        //             await newuow.CompleteAsync();
        //         }
        //     });
        //
        //     await Task.WhenAll(t1,t2);
        //     await _uow2.CompleteAsync();
        // }
    }

}


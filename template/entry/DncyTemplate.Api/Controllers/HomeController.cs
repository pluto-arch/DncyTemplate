﻿using DncyTemplate.Application.Models;
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
    [Route("api/[controller]")]
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
        public ResultDto TestLocalize([EmailAddress] string name)
        {
            var text = _stringLocalizer[SharedResource.Hello];
            return this.Success<string>(text);
        }


        [HttpGet("/uow")]
        public async Task<ResultDto> UnitOfWork2Test([FromServices] IEfRepository<Product> _repository, string name)
        {
            _logger.LogInformation("email is {email}", "aaaaa@qq.com");
            var id1 = SnowFlakeId.Generator.GetUniqueId();
            await using (_efUow.NewScopeAsync())
            {
                await _repository.InsertAsync(new Product
                {
                    Id = id1,
                    Name = "2",
                    Remark = "sdasdasdad",
                    CreationTime = DateTimeOffset.Now,
                });

                await using (_efUow.NewScopeAsync())
                {
                    await _repository.InsertAsync(new Product
                    {
                        Id = SnowFlakeId.Generator.GetUniqueId(),
                        Name = "3",
                        Remark = "sdasdasdad",
                        CreationTime = DateTimeOffset.Now,
                    });
                    await _efUow.CompleteAsync();
                }

                await using (_efUow.NewScopeAsync())
                {
                    await _repository.InsertAsync(new Product
                    {
                        Id = SnowFlakeId.Generator.GetUniqueId(),
                        Name = "3-1",
                        Remark = "sdasdasdad",
                        CreationTime = DateTimeOffset.Now,
                    });
                    await _efUow.CompleteAsync();
                }


                await _efUow.CompleteAsync();
            }

            var res = await _repository.FirstOrDefaultAsync(x => x.Id == id1);

            if (res != null)
            {
                res.Remark = "xxxxxxxxx";
            }

            await _repository.InsertAsync(new Product
            {
                Id = SnowFlakeId.Generator.GetUniqueId(),
                Name = "1",
                Remark = "sdasdasdad",
                CreationTime = DateTimeOffset.Now,
            });
            await _efUow.CompleteAsync();
            var text = _stringLocalizer[SharedResource.Hello];
            return this.Success<string>(text);
        }

        [HttpGet("loc")]
        [AllowAnonymous]
        public async Task<ResultDto> RpcCall()
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
        public ResultDto RateLimit([EmailAddress] string name)
        {
            var text = _stringLocalizer[SharedResource.Hello];
            return this.Success<string>(text);
        }
    }

}


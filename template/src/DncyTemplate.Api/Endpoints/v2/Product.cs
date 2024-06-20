﻿using System.ComponentModel.DataAnnotations;
using DncyTemplate.Application.AppServices.Product;
using DncyTemplate.Application.Command.Product;
using DncyTemplate.Application.Models;
using DncyTemplate.Application.Models.Product;
using Microsoft.AspNetCore.Authorization;

namespace DncyTemplate.Api.Controllers.v2
{

    [ApiController]
    [Route("api/v{version:apiVersion}/products")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    [Authorize]
    [AutoResolveDependency]
    public partial class Product : EndPointBase
    {
        [AutoInject]
        private readonly IProductAppService _productAppService;

        [AutoInject]
        private readonly IMediator _mediator;

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "ProductList")]
        [MapToApiVersion("2.0")]
        [Produces(typeof(IPagedList<ProductListItemDto>))]
        public async Task<ResultDto> ListAsync([FromQuery] ProductPagedRequest request)
        {
            var res = await _productAppService.GetListAsync(request);
            return this.Success(res);
        }

        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:required}")]
        [Produces(typeof(ProductDto))]
        public async Task<ResultDto> GetByIdAsync(string id)
        {
            var res = await _productAppService.GetAsync(id);
            if (res.Successed)
            {
                return this.Success(res.Data);
            }
            return this.Error(res.Errors.Message);
        }


        /// <summary>
        /// 新增产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ProductDto))]
        public async Task<ResultDto> CreateAsync([FromBody] CreateProductCommand request)
        {
            var productDto = await _mediator.Send(request);
            return this.Success(productDto);
        }


        /// <summary>
        /// 修改产品
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:required}")]
        [Produces(typeof(ProductDto))]
        public async Task<ResultDto> UpdateAsync([Required] string id, [FromForm] ProductUpdateRequest request)
        {
            var productDto = await _productAppService.UpdateAsync(id, request);
            return this.Success(productDto);
        }


        /// <summary>
        /// 删除产品
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:required}")]
        [Produces(typeof(ProductDto))]
        public async Task<ResultDto> DeleteAsync([Required] string id)
        {
            await _productAppService.DeleteAsync(id);
            return this.Success();
        }


        /// <summary>
        /// 按照名称查询 - efcore
        /// </summary>
        /// <returns></returns>
        [HttpGet("getByName")]
        [Produces(typeof(ProductDto))]
        public async Task<ResultDto> GetByNameAsync([Required] string name)
        {
            var res = await _productAppService.GetByNameAsync(name);
            return res.Match<ResultDto>(
                this.Success,
                this.Error
                );
        }

        /// <summary>
        /// 按照名称查询 - dapper
        /// </summary>
        /// <returns></returns>
        [HttpGet("getByNameWithDapper")]
        [Produces(typeof(ProductDto))]
        public async Task<ResultDto> GetByNameWithDapperAsync([Required] string name)
        {
            var res = await _productAppService.GetByNameWithDapperAsync(name);
            return res.Match<ResultDto>(
                this.Success,
                this.Error
            );
        }
    }
}

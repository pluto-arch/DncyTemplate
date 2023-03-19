using DncyTemplate.Application.AppServices.Product;
using DncyTemplate.Application.Command.Product;
using DncyTemplate.Application.Models.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Api.Controllers.v2
{

    [ApiController]
    [Route("api/v{version:apiVersion}/products")]
    [ApiVersion("2.0")]
    [Authorize]
    [AutoResolveDependency]
    public partial class ProductController : ControllerBase, IApiResultWapper
    {
        [AutoInject]
        private readonly IProductAppService _productAppService;

        [AutoInject]
        private readonly IMediator _mediator;

        /// <summary>
        /// 获取产品列表
        /// GET {PATH}?PageNo=1&pageSize=20&keyword=hahah&sorter={‘id’:'desc'}
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IPagedList<ProductListItemDto>))]
        public async Task<ApiResult> ListAsync(ProductPagedRequest request)
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
        public async Task<ApiResult> GetByIdAsync(string id)
        {
            var res = await _productAppService.GetAsync(id);
            return this.Success(res);
        }


        /// <summary>
        /// 新增产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ProductDto))]
        public async Task<ApiResult> CreateAsync([FromBody] CreateProductCommand request)
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
        public async Task<ApiResult> UpdateAsync([Required] string id, [FromForm] ProductUpdateRequest request)
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
        public async Task<ApiResult> DeleteAsync([Required] string id)
        {
            await _productAppService.DeleteAsync(id);
            return this.Success();
        }
    }
}

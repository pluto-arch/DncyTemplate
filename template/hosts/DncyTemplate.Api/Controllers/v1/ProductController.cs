
#if Tenant
using Dncy.MultiTenancy;
#endif
using DncyTemplate.Application.AppServices.Product;
using DncyTemplate.Application.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using AppModelAlias = DncyTemplate.Application.Models;

namespace DncyTemplate.Api.Controllers.v1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [AutoResolveDependency]
    [Authorize]
    public partial class ProductController : ControllerBase, IResponseWraps
    {
        [AutoInject]
        private readonly IProductAppService _productsRepository;

#if Tenant
        [AutoInject]
        private readonly ICurrentTenant _currentTenant;
#endif

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IPagedList<AppModelAlias.Product.ProductListItemDto>))]
        public async Task<ResultDto> GetAsync(int pageNo = 1, [Range(minimum: 1, maximum: 255, ErrorMessage = "PageSizeMessage")] byte pageSize = 20)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }
            var res = await _productsRepository.GetListAsync(new AppModelAlias.Product.ProductPagedRequest
            {
                PageNo = pageNo,
                PageSize = pageSize
            })!;
            return this.Success(new
            {
#if Tenant
                tenant = _currentTenant.Id,
#endif
                products = res
            });
        }


        /// <summary>
        /// 新增产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(AppModelAlias.Product.ProductDto))]
        public async Task<ResultDto> PostAsync([FromForm] string name)
        {
            var productDto = await _productsRepository.CreateAsync(new Application.Models.Product.ProductCreateRequest
            {
                Name = name,
                Remark = $"{name} remark"
            });
            return this.Success(productDto);
        }
    }
}

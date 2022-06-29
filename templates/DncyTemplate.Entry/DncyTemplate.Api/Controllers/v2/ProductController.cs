using DncyTemplate.Application.Models;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.DomainEvents.Product;
using DncyTemplate.Domain.Repository;
using DncyTemplate.Infra.EntityFrameworkCore.Extension;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Api.Controllers.v2
{
    
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class ProductController : ControllerBase, IApiResultWapper
    {
        private readonly IRepository<Product> _productsRepository;

        public ProductController(IRepository<Product> productsRepository)
        {
            _productsRepository = productsRepository;
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(IPagedList<Product>))]
        public async Task<ApiResult> GetAsync(int pageNo = 1, [Range(minimum: 1, maximum: 255, ErrorMessage = "PageSizeMessage")] byte pageSize = 20)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }
            var res = await _productsRepository.IgnoreQueryFilters().AsNoTracking().ToPagedListAsync(pageNo, pageSize);
            return this.Success(res);
        }


        /// <summary>
        /// 新增产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(Product))]
        public async Task<ApiResult> PostAsync([FromForm] string name)
        {
            var product = new Product
            {
                Id = $"P{DateTime.Now.Ticks}",
                Name = name,
                Remark = $"{name} remarks",
                CreationTime = DateTimeOffset.Now,
            };
            product.AddDevice(new Device
            {
                Name = "admin",
                SerialNo = "sssssss",
                Coordinate = (GeoCoordinate)"123.22,31.333",
                Online = true,
            });
            product.AddDomainEvent(new NewProductCreateDomainEvent(product));
            product = await _productsRepository.InsertAsync(product);
            return this.Success(product);
        }
    }
}

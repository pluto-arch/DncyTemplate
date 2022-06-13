using DncyTemplate.Application.Models;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.DomainEvents.Product;
using DncyTemplate.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[controller]")]
    [AutoResolveDependency]
    [ApiController]
    public partial class ProductController : ControllerBase
    {
        [AutoInject]
        private readonly IRepository<Product> _productsRepository;

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(List<Product>))]
        public async Task<ApiResult> GetAsync()
        {
            var res = await _productsRepository.AsNoTracking().ToListAsync();
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

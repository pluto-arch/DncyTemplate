using DncyTemplate.Application.Permission;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
        [Authorize(ProductPermission.Product.Default)]
        public async Task<IEnumerable<Product>> GetAsync()
        {
            return await _productsRepository.AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// 新增产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Product> PostAsync([FromForm]string name)
        {
            var product=new Product
            {
                Id = $"P{DateTime.Now.Ticks}",
                Name = name,
                Remark = $"{name} remarks",
                CreationTime = DateTimeOffset.Now,
            };
            product = await _productsRepository.InsertAsync(product);
            return product;
        }
    }
}

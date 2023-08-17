using Bogus;
#if Tenant
using Dncy.MultiTenancy.Model;
using Dncy.MultiTenancy;
#endif
using DncyTemplate.Application.AppServices.Product;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Infra.Repository;
using DncyTemplate.Infra.EntityFrameworkCore.Repository;
using Moq;
using DncyTemplate.Domain.Infra;
using System.Linq.Expressions;

namespace DncyTemplate.UnitTest
{
    public class DemoTest : BaseTest
    {

        [Test]
        public async Task Test()
        {
#if Tenant
            var tenant = ServiceProvider.GetService<ICurrentTenant>();
            using (tenant.Change(new TenantInfo("T20210602000003", "")))
            {
#endif

                var productService = ServiceProvider.GetService<IProductAppService>();

                var res = await productService.GetListAsync(new ProductPagedRequest
                {
                    PageNo = 1,
                    PageSize = 20,
                });

                Assert.That(res != null, Is.True);
#if Tenant
            }
#endif
        }


        [Test]
        public async Task MoqTest()
        {
            var fakeProducts = new Faker<Product>()
                .RuleFor(x => x.Id, s => s.Random.Guid().ToString("N"))
                .RuleFor(x => x.Name, f => f.Lorem.Word())
                .RuleFor(x => x.CreationTime, f => f.Date.Soon())
                .Generate(1000);


            var first = fakeProducts.First();
            var productQuerable = fakeProducts.AsQueryable();


            var mockAsyncExecuter = new Mock<IAsyncQueryableProvider>();
            mockAsyncExecuter.Setup(x => x.FirstOrDefaultAsync(It.IsAny<IQueryable<Product>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(first);


            var mockRepository= new Mock<IEfRepository<Product,string>>();
            mockRepository.Setup(r => r.AsyncExecuter).Returns(mockAsyncExecuter.Object);
            mockRepository.Setup(r => r.QuerySet).Returns(productQuerable);



            var result = await mockRepository.Object.FirstOrDefaultAsync();
            Assert.That(result, Is.Not.Null);

            Assert.That(first.Name, Is.EqualTo(result.Name));
        }

    }
}

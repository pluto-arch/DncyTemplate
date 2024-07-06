#if Tenant
using Dotnetydd.MultiTenancy.Model;
using Dotnetydd.MultiTenancy;
#endif
using DncyTemplate.Application.AppServices.Product;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Infra.Repository;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Uow;

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

                await productService.CreateAsync(new ProductCreateRequest
                {
                    Id = "xxx",
                    Name = "Hello",
                    Remark = "ddddd"
                });

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
        public async Task UnitOfWorkTest()
        {
            var uow = ServiceProvider.GetService<IUnitOfWork<DncyTemplateDbContext>>();

            var code = uow.GetHashCode();

            var repa = uow.Resolve<IEfRepository<Product>>();


            await using var subUow = uow.BeginNew();
            var code2=subUow.ServiceProvider.GetHashCode();
            Assert.That(code!=code2,Is.True);
            var rep2= subUow.Resolve<IEfRepository<Product>>();
            Assert.That(repa != rep2, Is.True);
            Assert.That(repa.DataContext.GetHashCode()!=rep2.DataContext.GetHashCode(),Is.True);

        }

    }
}

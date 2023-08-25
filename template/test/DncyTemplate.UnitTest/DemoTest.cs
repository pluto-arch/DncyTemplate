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
using DncyTemplate.Domain.Infra;

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

    }
}

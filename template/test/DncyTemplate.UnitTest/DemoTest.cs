#if Tenant
using Dotnetydd.MultiTenancy.Model;
using Dotnetydd.MultiTenancy;
#endif
using DncyTemplate.Application.AppServices.Product;
using DncyTemplate.Application.Models.Product;

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

    }
}

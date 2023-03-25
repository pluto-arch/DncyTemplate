using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dncy.MultiTenancy;
using Dncy.MultiTenancy.Model;
using DncyTemplate.Domain.Aggregates.Product;
using Microsoft.Extensions.DependencyInjection;
using DncyTemplate.Infra.EntityFrameworkCore;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Infra.Utils;

namespace DncyTemplate.Test
{
    public class EfInfraTest:Init
    {

        [Test]
        public void ResolveUow()
        {
            var uow = ServiceProvider.GetService<EfUnitOfWork<DncyTemplateDbContext>>();
            Assert.IsNotNull(uow);
        }



        [Test]
        public async Task UowResolveDefaultRepository()
        {
            var uow = ServiceProvider.GetService<EfUnitOfWork<DncyTemplateDbContext>>();
            Assert.IsNotNull(uow);

            var productRep = uow.Repository<Product>();
            Assert.IsNotNull(productRep);

            var currentTenant = ServiceProvider.GetService<ICurrentTenant>();
            Assert.IsNotNull(currentTenant);
            using (currentTenant.Change(new TenantInfo
                   {
                       Id = "T20210602000002",
                       IsAvaliable = false
                   }))
            {
                var productList = await productRep.GetListAsync();
                Assert.IsNotNull(productList);

                await productRep.InsertAsync(new Product
                {
                    Id = SnowFlakeId.Generator.GetUniqueId(),
                    Name = "change22222",
                    Remark = "sdasdasdad",
                    CreationTime = DateTimeOffset.Now,
                });
                await uow.CompleteAsync();

                productList = await productRep.GetListAsync();
                Assert.IsTrue(productList is { Count:>0});
            }
        }
    }
}

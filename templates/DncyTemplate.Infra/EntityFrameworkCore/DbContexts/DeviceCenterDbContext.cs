using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.UnitOfWork;
using DncyTemplate.Infra.EntityFrameworkCore.EntityTypeConfig;
using Microsoft.EntityFrameworkCore;

namespace DncyTemplate.Infra.EntityFrameworkCore.DbContexts;

public class DeviceCenterDbContext : BaseDbContext<DeviceCenterDbContext>, IUowDbContext
{
    public DeviceCenterDbContext(DbContextOptions<DeviceCenterDbContext> options)
        : base(options)
    {
    }


    public DbSet<Product> Products { get; set; }

    public DbSet<Device> Device { get; set; }

    public DbSet<DeviceTag> DeviceTag { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // 不能去除，对租户，软删除过滤器
        modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration())
            .ApplyConfiguration(new DeviceTagEntityTypeConfiguration())
            .ApplyConfiguration(new ProductEntityTypeConfiguration());
    }


    ///// <summary>
    ///// 动态获取efcore的IModel  会造成性能下降
    ///// 这里返回租户提供程序，从而每次都走 OnModelCreating  表名就可以按租户进行分表了
    ///// TabRoute 是dbcontext公开的分表路由，这个可以自定义
    ///// </summary>
    //public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    //{
    //    public object Create(DbContext context)
    //    {
    //        return context is DeviceCenterDbContext shardingContext
    //            ? (context.GetType(), TabRoute: shardingContext.TabRoute)
    //            : (object)context.GetType();
    //    }
    //}

}
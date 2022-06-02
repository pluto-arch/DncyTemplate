using Dncy.MultiTenancy;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Aggregates.System;
using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.UnitOfWork;
using DncyTemplate.Infra.EntityFrameworkCore.EntityTypeConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

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

    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // 不能去除，对租户，软删除过滤器
        modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration())
            .ApplyConfiguration(new DeviceTagEntityTypeConfiguration())
            .ApplyConfiguration(new ProductEntityTypeConfiguration())
            .ApplyConfiguration(new PermissionEntityTypeConfiguration());
    }


    /*
    /// <summary>
    /// 动态执行 Create ，如果返回和上一次不一样，则会重新走 dbcontext的 OnModelCreating 从而可以实现分表，但是会影响性能
    /// </summary>
    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    {
        private readonly ICurrentTenant _currentTenant;

        public DynamicModelCacheKeyFactory(ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
        }

        public object Create(DbContext context)
        {
            if (context is DeviceCenterDbContext dynamicContext)
            {
                dynamicContext.TabRoute = _currentTenant.Id;
                return (context.GetType(), _currentTenant.Id);
            }
            return (object)context.GetType();
        }

#if NET6_0
        public object Create(DbContext context, bool designTime)
        {
            if (context is DeviceCenterDbContext dynamicContext)
            {
                dynamicContext.TabRoute = _currentTenant.Id;
                return (context.GetType(), _currentTenant.Id);
            }
            return (object)context.GetType();
        }
#endif

    }
    */


}
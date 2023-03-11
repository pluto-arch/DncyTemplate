using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Aggregates.System;
using DncyTemplate.Infra.EntityFrameworkCore.EntityTypeConfig;


namespace DncyTemplate.Infra.EntityFrameworkCore.DbContexts;

public class DncyTemplateDbContext : BaseDbContext<DncyTemplateDbContext>
{
    public DncyTemplateDbContext(DbContextOptions<DncyTemplateDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<Device> Device { get; set; }

    public DbSet<DeviceTag> DeviceTag { get; set; }

    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        //分表使用 - 替换ef的缓存，造成性能会下降 并且不能使用 AddDbContextPool.
        // optionsBuilder.ReplaceService<IModelCacheKeyFactory,DynamicModelCacheKeyFactory>();
        // optionsBuilder.ReplaceService<IModelCustomizer, UserModelCustomizer>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // 不能去除，对租户，软删除过滤器
        modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration())
            .ApplyConfiguration(new DeviceTagEntityTypeConfiguration())
            .ApplyConfiguration(new ProductEntityTypeConfiguration())
            .ApplyConfiguration(new PermissionEntityTypeConfiguration());
    }


    #region 分表

    //    public string TabRoute { get; internal set; }

    //    /// <summary>
    //    /// 动态执行 Create ，如果返回和上一次不一样，则会重新走 dbcontext的 OnModelCreating 从而可以实现分表，但是会影响性能
    //    /// </summary>
    //    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    //    {
    //        public object Create(DbContext context)
    //        {
    //            if (context is DncyTemplateDbContext dynamicContext)
    //            {
    //                var ic = dynamicContext.GetService<ICurrentTenant>();
    //                var dd = $"{DateTime.Now:yyyyMM}";
    //                dynamicContext.TabRoute = dd;
    //                return (context.GetType(), dd);
    //            }
    //            return (object)context.GetType();
    //        }

    //#if NET5_0_OR_GREATER
    //        public object Create(DbContext context, bool designTime)
    //        {
    //            if (context is DncyTemplateDbContext dynamicContext)
    //            {
    //                var ic = dynamicContext.GetService<ICurrentTenant>();
    //                var dd = $"{DateTime.Now:yyyyMM}";
    //                dynamicContext.TabRoute = dd;
    //                return (context.GetType(), dd);
    //            }
    //            return (object)context.GetType();
    //        }
    //#endif

    //    }

    //    public class UserModelCustomizer : ModelCustomizer
    //    {
    //        public UserModelCustomizer(ModelCustomizerDependencies dependencies) : base(dependencies)
    //        {
    //        }


    //        public override void Customize(ModelBuilder modelBuilder, DbContext context)
    //        {
    //            base.Customize(modelBuilder, context);
    //            var dbContextBase = context as DncyTemplateDbContext;
    //            // TODO 挑拣需要的表进行表明映射
    //            modelBuilder.Entity<Product>().ToTable($"product_{dbContextBase.TabRoute}");
    //        }
    //    }

    #endregion


}
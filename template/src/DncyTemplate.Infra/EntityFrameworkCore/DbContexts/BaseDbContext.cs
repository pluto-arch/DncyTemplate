#if Tenant
using Dncy.MultiTenancy;
#endif
using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Infra.UnitOfWork;
using DncyTemplate.Infra.EntityFrameworkCore.Extension;


namespace DncyTemplate.Infra.EntityFrameworkCore.DbContexts;

public class BaseDbContext<TContext> : DbContext
    where TContext : DbContext, IDataContext
{
    public BaseDbContext(DbContextOptions<TContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType item in modelBuilder.Model.GetEntityTypes())
        {
#if Tenant
            // 多租户
            if (item.ClrType.IsAssignableTo(typeof(IMultiTenant)))
            {
                modelBuilder.Entity(item.ClrType)
                    .AddQueryFilter<IMultiTenant>(e => e.TenantId == this.GetService<ICurrentTenant>().Id);
            }
#endif
            // 软删除
            if (item.ClrType.IsAssignableTo(typeof(ISoftDelete)))
            {
                modelBuilder.Entity(item.ClrType).AddQueryFilter<ISoftDelete>(e => !e.Deleted);
            }
        }
    }
}
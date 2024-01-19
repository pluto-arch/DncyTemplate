using DncyTemplate.Domain.Aggregates.EventLogs;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Aggregates.System;
using DncyTemplate.Infra.EntityFrameworkCore.EntityTypeConfig;
using DncyTemplate.Uow;


namespace DncyTemplate.Infra.EntityFrameworkCore.DbContexts;

public class DncyTemplateDbContext : BaseDbContext<DncyTemplateDbContext>, IDataContext
{
    public DncyTemplateDbContext(DbContextOptions<DncyTemplateDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Device> Device { get; set; }

    public virtual DbSet<DeviceTag> DeviceTag { get; set; }

    public virtual DbSet<PermissionGrant> PermissionGrants { get; set; }

    public virtual DbSet<IntegrationEventLogEntry> IntegrationEventLogEntry { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // 不能去除，对租户，软删除过滤器
        modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration())
            .ApplyConfiguration(new DeviceTagEntityTypeConfiguration())
            .ApplyConfiguration(new ProductEntityTypeConfiguration())
            .ApplyConfiguration(new PermissionEntityTypeConfiguration());

        modelBuilder.Entity<IntegrationEventLogEntry>().ToTable("IntegrationEventLog");
    }
}

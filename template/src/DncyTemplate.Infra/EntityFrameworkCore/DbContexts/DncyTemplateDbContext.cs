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

    public DbSet<Product> Products { get; set; }

    public DbSet<Device> Device { get; set; }

    public DbSet<DeviceTag> DeviceTag { get; set; }

    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // 不能去除，对租户，软删除过滤器
        modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration())
            .ApplyConfiguration(new DeviceTagEntityTypeConfiguration())
            .ApplyConfiguration(new ProductEntityTypeConfiguration())
            .ApplyConfiguration(new PermissionEntityTypeConfiguration());
    }
}

public class DncyTemplateDb2Context : BaseDbContext<DncyTemplateDb2Context>, IDataContext
{
    public DncyTemplateDb2Context(DbContextOptions<DncyTemplateDb2Context> options)
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
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // 不能去除，对租户，软删除过滤器
        modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration())
            .ApplyConfiguration(new DeviceTagEntityTypeConfiguration())
            .ApplyConfiguration(new ProductEntityTypeConfiguration())
            .ApplyConfiguration(new PermissionEntityTypeConfiguration());
    }
}
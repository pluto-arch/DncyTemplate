using DncyTemplate.Infra.EntityFrameworkCore.EntityTypeConfig;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DncyTemplate.Infra.EntityFrameworkCore.DbContexts;


public class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<DeviceCenterMigrationDbContext>
{
    public DeviceCenterMigrationDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<DeviceCenterMigrationDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlServer(@"Server=127.0.0.1,1433;Database=Pnct_Default;User Id=sa;Password=970307lBX;Trusted_Connection = False;");
        return new DeviceCenterMigrationDbContext(optionsBuilder.Options);
    }
}


/// <summary>
/// ef迁移使用
/// </summary>
public class DeviceCenterMigrationDbContext : DbContext
{
    public DeviceCenterMigrationDbContext(DbContextOptions<DeviceCenterMigrationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration())
            .ApplyConfiguration(new DeviceTagEntityTypeConfiguration())
            .ApplyConfiguration(new ProductEntityTypeConfiguration());
    }
}

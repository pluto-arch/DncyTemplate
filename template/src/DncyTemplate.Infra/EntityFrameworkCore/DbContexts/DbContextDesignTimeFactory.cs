using DncyTemplate.Infra.EntityFrameworkCore.EntityTypeConfig;
using Microsoft.EntityFrameworkCore.Design;

namespace DncyTemplate.Infra.EntityFrameworkCore.DbContexts;


public class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<DncyTemplateMigrationDbContext>
{
    public DncyTemplateMigrationDbContext CreateDbContext(string[] args)
    {
        return new DncyTemplateMigrationDbContext();
    }
}


/// <summary>
/// ef迁移使用
/// </summary>
public class DncyTemplateMigrationDbContext : DbContext
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(@"Server=localhost,1433;Database=Pnct_Default;User Id=sa;Password=970307lBx;Trusted_Connection = False;TrustServerCertificate=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration())
            .ApplyConfiguration(new DeviceTagEntityTypeConfiguration())
            .ApplyConfiguration(new ProductEntityTypeConfiguration())
            .ApplyConfiguration(new PermissionEntityTypeConfiguration());
    }
}

using DncyTemplate.Infra.EntityFrameworkCore.EntityTypeConfig;
using Microsoft.EntityFrameworkCore.Design;

namespace DncyTemplate.Infra.EntityFrameworkCore.DbContexts;


public class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<DncyTemplateMigrationDbContext>
{
    public DncyTemplateMigrationDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<DncyTemplateMigrationDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlServer(@"Server=localhost,1433;Database=Pnct_Default;User Id=sa;Password=970307lBx;Trusted_Connection = False;TrustServerCertificate=true");
        return new DncyTemplateMigrationDbContext(optionsBuilder.Options);
    }
}


/// <summary>
/// ef迁移使用
/// </summary>
public class DncyTemplateMigrationDbContext : DbContext
{
    public DncyTemplateMigrationDbContext(DbContextOptions<DncyTemplateMigrationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration())
            .ApplyConfiguration(new DeviceTagEntityTypeConfiguration())
            .ApplyConfiguration(new ProductEntityTypeConfiguration())
            .ApplyConfiguration(new PermissionEntityTypeConfiguration());
    }
}

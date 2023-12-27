using DncyTemplate.Infra.EntityFrameworkCore.EntityTypeConfig;
using Microsoft.EntityFrameworkCore.Design;

namespace DncyTemplate.Infra.EntityFrameworkCore.DbContexts;


public class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<DncyTemplateDbContext>
{
    public DncyTemplateDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DncyTemplateDbContext>();
        optionsBuilder.UseSqlServer(@"Server=localhost,1433;Database=DncyTemplateDb;User Id=sa;Password=970307lBx;Trusted_Connection = False;TrustServerCertificate=true");
        return new DncyTemplateDbContext(optionsBuilder.Options);
    }
}

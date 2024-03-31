using DncyTemplate.Infra.EntityFrameworkCore.EntityTypeConfig;
using Microsoft.EntityFrameworkCore.Design;

namespace DncyTemplate.Infra.EntityFrameworkCore.DbContexts;


public class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<DncyTemplateDbContext>
{
    public DncyTemplateDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DncyTemplateDbContext>();
        optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=DncyTemplateDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        return new DncyTemplateDbContext(optionsBuilder.Options);
    }
}

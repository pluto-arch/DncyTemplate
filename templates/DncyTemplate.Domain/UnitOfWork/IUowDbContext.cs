namespace DncyTemplate.Domain.UnitOfWork;

public interface IUowDbContext : IDisposable, IAsyncDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();
}
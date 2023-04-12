namespace DncyTemplate.Domain.Infra.UnitOfWork
{
    public interface IDataContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
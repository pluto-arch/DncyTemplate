using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Repository;

namespace DncyTemplate.Domain.UnitOfWork;

public interface IUnitOfWork: IAsyncDisposable, IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();

    IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;

    IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;

    TRep GetCustomRepository<TRep>() where TRep : IRepository;
}


public interface IUnitOfWork<T> : IUnitOfWork where T : IUowDbContext
{
    T Context { get; }

    T GetDbContext();
}
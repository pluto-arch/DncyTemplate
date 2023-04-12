using DncyTemplate.Domain.Infra.Repository;

namespace DncyTemplate.Domain.Infra.UnitOfWork
{
    public interface IUnitOfWork<TContext> : IDisposable, IAsyncDisposable
        where TContext : IDataContext
    {

        TContext DbContext();


        IDisposable NewScope();


        IAsyncDisposable NewScopeAsync();

        /// <summary>
        /// 获取仓储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEfRepository<T> EfRepository<T>() where T : class, IEntity;

        /// <summary>
        /// 获取仓储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">实体主键</typeparam>
        /// <returns></returns>
        IEfRepository<T, TKey> EfRepository<T, TKey>() where T : class, IEntity;


        int Complete();


        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
}

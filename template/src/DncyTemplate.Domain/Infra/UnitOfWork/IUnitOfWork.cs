using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Infra.Repository;

namespace DncyTemplate.Uow
{


    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {

        IServiceProvider ServiceProvider { get; }
        
        
        IDataContext Context { get; }

        /// <summary>
        /// 新的范围作用域
        /// </summary>
        /// <returns></returns>
        IDisposable Change();


        /// <summary>
        /// 使用新的单元
        /// </summary>
        /// <returns></returns>
        IUnitOfWork BeginNew();

        
        /// <summary>
        /// 完成unitofwork
        /// </summary>
        /// <returns></returns>
        int Complete();

        /// <summary>
        /// 完成unitofwork
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 获取Efcore仓储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEfRepository<T> GetEfRepository<T>() where T : class, IEntity;

        /// <summary>
        /// 获取Efcore仓储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">实体主键</typeparam>
        /// <returns></returns>
        IEfRepository<T, TKey> GetEfRepository<T, TKey>() where T : class, IEntity;
        
        event Action OnDisposed;
    }


    public interface IUnitOfWork<TContext> : IUnitOfWork
        where TContext : IDataContext
    {
        TContext DbContext();
    }
}

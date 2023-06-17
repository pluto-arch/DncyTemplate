using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Infra.Repository;

namespace DncyTemplate.Uow
{


    public interface IUnitOfWork:IDisposable, IAsyncDisposable
    {

        IDataContext Context { get; }

        /// <summary>
        /// 新的范围作用域
        /// </summary>
        /// <returns></returns>
        IDisposable NewScope();

        /// <summary>
        /// 新的异步范围作用域
        /// </summary>
        /// <returns></returns>
        IAsyncDisposable NewScopeAsync();

        int Complete();

        /// <summary>
        /// 完成uow
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
    }


    public interface IUnitOfWork<TContext> : IUnitOfWork
        where TContext : IDataContext
    {
        TContext DbContext();
    }
}

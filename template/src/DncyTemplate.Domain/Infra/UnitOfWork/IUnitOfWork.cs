using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Infra.Repository;

namespace DncyTemplate.Uow
{


    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IServiceProvider ServiceProvider { get; }

        IDataContext Context { get; }

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
        /// 获取通用仓储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IGenericRepository<T> GetRepository<T>() where T : class, IEntity;

        /// <summary>
        /// 从容器中解析服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();

        event Action OnDisposed;
    }


    public interface IUnitOfWork<out TContext> : IUnitOfWork
        where TContext : IDataContext
    {
        TContext DbContext();
    }
}

using DncyTemplate.Domain.Infra;
using DncyTemplate.Infra.EntityFrameworkCore.Repository;

namespace DncyTemplate.Infra.EntityFrameworkCore
{

    public class EfUnitOfWork<TContext> : IDisposable, IAsyncDisposable
        where TContext : DbContext
    {
        private bool disposedValue;
        private IServiceProvider _serviceProvider;


        public EfUnitOfWork(IServiceProvider serviceProvider, TContext rootDbContext)
        {
            _serviceProvider = serviceProvider;
            DbContext = rootDbContext;
        }

        /// <summary>
        /// 新作用域
        /// </summary>
        /// <returns></returns>
        public IDisposable NewScope()
        {
            var previousDbContext = DbContext;
            var previousProvider = _serviceProvider;

            var scoped = _serviceProvider.CreateScope();
            _serviceProvider = scoped.ServiceProvider;
            var context = _serviceProvider.GetRequiredService<TContext>();
            DbContext = context;
            return new DisposeAction(() =>
            {
                DbContext = previousDbContext;
                _serviceProvider = previousProvider;
                scoped?.Dispose();
                context?.Dispose();
            });
        }

        public IEfRepository<T> Repository<T>() where T : class, IEntity
        {
            return _serviceProvider.GetRequiredService<IEfRepository<T>>();
        }

        public IEfRepository<T, TKey> Repository<T, TKey>() where T : class, IEntity
        {
            return _serviceProvider.GetRequiredService<IEfRepository<T, TKey>>();
        }

        public int Complete()
        {
            return DbContext.SaveChanges();
        }
        public Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public TContext DbContext { get; private set; }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DbContext?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public ValueTask DisposeAsync()
        {
            Dispose(true); return ValueTask.CompletedTask;
        }
    }
}

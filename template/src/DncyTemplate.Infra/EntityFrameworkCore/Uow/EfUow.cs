using DncyTemplate.Domain.Infra;
using DncyTemplate.Infra.EntityFrameworkCore.Repository;
using System.Collections.Concurrent;

namespace DncyTemplate.Infra.EntityFrameworkCore
{
    public class EfUow<TContext> : IDisposable, IAsyncDisposable
        where TContext : DbContext
    {
        private bool disposedValue;
        private IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<Type, object> _repositories = new ConcurrentDictionary<Type, object>();
        private readonly ConcurrentDictionary<Type, object> _keyRepositories = new ConcurrentDictionary<Type, object>();

        public EfUow(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            DbContext = _serviceProvider.GetService<TContext>();
        }

        /// <summary>
        /// 切换子uow作用域
        /// </summary>
        /// <returns></returns>
        public IDisposable Change()
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

        public IEfGenericRepository<TContext, T> Repository<T>() where T : class, IEntity
        {
            if (_repositories.Keys.Contains(typeof(T)))
            {
                return _repositories[typeof(T)] as IEfGenericRepository<TContext, T>;
            }
            var repository = ActivatorUtilities.CreateInstance<EfGenericRepository<TContext, T>>(_serviceProvider, this);
            _repositories.AddOrUpdate(typeof(T), () => repository, (k, _) => repository);
            return repository;
        }

        public IEfKeyedRepository<TContext, T, TKey> Repository<T, TKey>() where T : class, IEntity
        {
            if (_keyRepositories.Keys.Contains(typeof(T)))
            {
                return _keyRepositories[typeof(T)] as IEfKeyedRepository<TContext, T, TKey>;
            }
            var repository = ActivatorUtilities.CreateInstance<EfKeyedRepository<TContext, T, TKey>>(_serviceProvider, this);
            _keyRepositories.AddOrUpdate(typeof(T), () => repository, (k, _) => repository);
            return repository;
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
                    _repositories.Clear();
                    _keyRepositories.Clear();
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

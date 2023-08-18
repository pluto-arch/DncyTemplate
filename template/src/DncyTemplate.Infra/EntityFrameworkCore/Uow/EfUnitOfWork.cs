using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Infra.Repository;

namespace DncyTemplate.Uow.EntityFrameworkCore
{
    public class EfUnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext : DbContext, IDataContext
    {
        private AsyncLocal<bool> _disposedValue = new AsyncLocal<bool>();
        private AsyncLocal<IServiceProvider> _serviceProvider = new AsyncLocal<IServiceProvider>();
        private AsyncLocal<TContext> _context = new AsyncLocal<TContext>();

        public EfUnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider.Value = serviceProvider;
            _context.Value = serviceProvider.GetService<TContext>();
        }

        public IServiceProvider ServiceProvider => _serviceProvider.Value;

        /// <inheritdoc />
        public IDataContext Context => _context.Value;

        /// <inheritdoc />
        public IDisposable Change()
        {
            var previousDbContext = _context.Value;
            var previousProvider = _serviceProvider.Value;

            var scope = _serviceProvider.Value.CreateScope();
            _serviceProvider.Value = scope.ServiceProvider;
            var newContext = _serviceProvider.Value.GetRequiredService<TContext>();
            _context.Value = newContext;

            return new DisposeAction(() =>
            {
                _context.Value = previousDbContext;
                _serviceProvider.Value = previousProvider;
                scope.Dispose();
                if (newContext != null)
                {
                    newContext.Dispose();
                    newContext = null;
                }
            });
        }

        public IUnitOfWork BeginNew()
        {
            var scope = _serviceProvider.Value.CreateScope();
            var newUow = scope.ServiceProvider.GetRequiredService<IUnitOfWork<TContext>>();
            newUow.OnDisposed += () =>
            {
                scope.Dispose();
                newUow = null;
            };
            return newUow;
        }


        /// <inheritdoc />
        public TContext DbContext() => _context.Value;

        public event Action OnDisposed;


        public IEfRepository<T> GetEfRepository<T>() where T : class, IEntity
        {
            return _serviceProvider.Value.GetRequiredService<IEfContextRepository<TContext, T>>();
        }

        public IEfRepository<T, TKey> GetEfRepository<T, TKey>() where T : class, IEntity
        {
            return _serviceProvider.Value.GetRequiredService<IEfContextRepository<TContext, T, TKey>>();
        }

        public int Complete()
        {
            return _context.Value.SaveChanges();
        }

        public Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return _context.Value.SaveChangesAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue.Value)
            {
                if (disposing)
                {
                    _context?.Value?.Dispose();
                    OnDisposed?.Invoke();
                }

                _disposedValue.Value = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public ValueTask DisposeAsync()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            return ValueTask.CompletedTask;
        }
    }
}
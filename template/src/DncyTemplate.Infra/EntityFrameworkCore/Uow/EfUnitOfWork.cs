using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Infra.Repository;

namespace DncyTemplate.Uow.EntityFrameworkCore
{
    public class EfUnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext : DbContext, IDataContext
    {
        private bool _disposedValue;
        private IServiceProvider _serviceProvider;
        private TContext _context;

        public EfUnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _context = serviceProvider.GetRequiredService<TContext>();
        }

        public IServiceProvider ServiceProvider => _serviceProvider;

        /// <inheritdoc />
        public IDataContext Context => _context;

        public IUnitOfWork BeginNew()
        {
            var scope = _serviceProvider.CreateScope();
            var newUow = scope.ServiceProvider.GetRequiredService<IUnitOfWork<TContext>>();
            newUow.OnDisposed += () =>
            {
                scope.Dispose();
                scope = null;
                newUow = null;
            };
            return newUow;
        }


        /// <inheritdoc />
        public TContext DbContext() => _context;

        public event Action OnDisposed;


        public IEfRepository<T> GetEfRepository<T>() where T : class, IEntity
        {
            return _serviceProvider.GetRequiredService<IEfContextRepository<TContext, T>>();
        }

        public IEfRepository<T, TKey> GetEfRepository<T, TKey>() where T : class, IEntity
        {
            return _serviceProvider.GetRequiredService<IEfContextRepository<TContext, T, TKey>>();
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _context?.Dispose();
                    _context = null;
                    _serviceProvider = null;
                    OnDisposed?.Invoke();
                }

                _disposedValue = true;
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
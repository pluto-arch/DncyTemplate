using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Infra.Repository;

namespace DncyTemplate.Uow.EntityFrameworkCore
{

    public class EfUnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext : DbContext, IDataContext
    {
        private bool _disposedValue;
        private IServiceProvider _serviceProvider;
        private AsyncLocal<TContext> _context=new AsyncLocal<TContext>();
        private readonly TContext _rootDbContext;

        public EfUnitOfWork(IServiceProvider serviceProvider, TContext rootDbContext)
        {
            _serviceProvider = serviceProvider;
            _rootDbContext = rootDbContext;
            _context.Value = rootDbContext;
        }


        /// <inheritdoc />
        public IDataContext Context => DbContext();

        /// <inheritdoc />
        public IDisposable NewScope()
        {
            var previousDbContext = _context.Value;
            var previousProvider = _serviceProvider;
            var scoped = _serviceProvider.CreateScope();
            _serviceProvider = scoped.ServiceProvider;
            var newContext = _serviceProvider.GetRequiredService<TContext>();
            _context.Value = newContext;
            return new DisposeAction(() =>
            {
                _context.Value = previousDbContext;
                _serviceProvider = previousProvider;
                scoped.Dispose();
                if (newContext!=null)
                {
                    newContext.Dispose();
                    newContext = null;
                }
            });
        }

        /// <inheritdoc />
        public IAsyncDisposable NewScopeAsync()
        {
            var previousDbContext = _context.Value;
            var previousProvider = _serviceProvider;
            var scoped = _serviceProvider.CreateScope();
            _serviceProvider = scoped.ServiceProvider;
            var newContext = _serviceProvider.GetRequiredService<TContext>();
            _context.Value = newContext;
            return new AsyncDisposeAction(async () =>
            {
                _context.Value = previousDbContext;
                _serviceProvider = previousProvider;
                scoped.Dispose();
                if (newContext!=null)
                {
                    await newContext.DisposeAsync();
                    newContext = null;
                }
            });
        }

        /// <inheritdoc />
        public TContext DbContext()
        {
            if (_context.Value==null)
            {
                _context.Value = _rootDbContext;
            }
            return _context.Value;
        }


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
            return _context.Value.SaveChanges();
        }
        public Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return _context.Value.SaveChangesAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _context?.Value?.Dispose();
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

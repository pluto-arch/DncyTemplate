using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Infra.Repository;
using DncyTemplate.Domain.Infra.UnitOfWork;

namespace DncyTemplate.Infra.EntityFrameworkCore
{

    public class EfUnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext : DbContext, IDataContext
    {
        private bool disposedValue;
        private IServiceProvider _serviceProvider;
        private TContext _context;


        public EfUnitOfWork(IServiceProvider serviceProvider, TContext rootDbContext)
        {
            _serviceProvider = serviceProvider;
            _context = rootDbContext;
        }


        public IDisposable NewScope()
        {
            return SetDisposable();
        }

        public IAsyncDisposable NewScopeAsync()
        {
            return SetAsyncDisposable();
        }

        public TContext DbContext() => _context;


        public IEfRepository<T> EfRepository<T>() where T : class, IEntity
        {
            return _serviceProvider.GetRequiredService<IEfRepository<T>>();
        }

        public IEfRepository<T, TKey> EfRepository<T, TKey>() where T : class, IEntity
        {
            return _serviceProvider.GetRequiredService<IEfRepository<T, TKey>>();
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
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context?.Dispose();
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
            Dispose(true);
            return ValueTask.CompletedTask;
        }


        private IAsyncDisposable SetAsyncDisposable()
        {
            var previousDbContext = _context;
            var previousProvider = _serviceProvider;
            var scoped = _serviceProvider.CreateScope();
            _serviceProvider = scoped.ServiceProvider;
            var context = _serviceProvider.GetRequiredService<TContext>();
            _context = context;
            return new AsyncDisposeAction(() =>
            {
                _context = previousDbContext;
                _serviceProvider = previousProvider;
                scoped.Dispose();
                context?.Dispose();
                return Task.CompletedTask;
            });
        }
        private IDisposable SetDisposable()
        {
            var previousDbContext = _context;
            var previousProvider = _serviceProvider;
            var scoped = _serviceProvider.CreateScope();
            _serviceProvider = scoped.ServiceProvider;
            var context = _serviceProvider.GetRequiredService<TContext>();
            _context = context;
            return new DisposeAction(() =>
            {
                _context = previousDbContext;
                _serviceProvider = previousProvider;
                scoped.Dispose();
                context?.Dispose();
            });
        }
    }
}

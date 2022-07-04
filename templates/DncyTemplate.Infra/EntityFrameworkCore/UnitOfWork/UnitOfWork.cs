using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Repository;
using DncyTemplate.Domain.UnitOfWork;

using Microsoft.Extensions.DependencyInjection;

namespace DncyTemplate.Infra.EntityFrameworkCore.UnitOfWork;

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : IUowDbContext
{
    private readonly TContext _rootContext;
    private readonly IServiceProvider _rootServiceProvider;
    private TContext _currenDbContext;
    private IServiceProvider _serviceProvider;
    private bool disposedValue;

    public UnitOfWork(TContext ctx, IServiceProvider serviceProvider, UnitOfWorkScopeManager uowScopeManager)
    {
        _currenDbContext = ctx ?? throw new ArgumentNullException(nameof(ctx));
        _rootContext = ctx;
        _rootServiceProvider = serviceProvider;

        if (uowScopeManager != null)
        {
            uowScopeManager.OnScopeChanged += UowScopeManager_OnScopeChanged;
        }
    }

    public TContext Context => GetDbContext();

    
    private async Task UowScopeManager_OnScopeChanged(IServiceProvider provider)
    {
        _serviceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        _currenDbContext = _serviceProvider.GetRequiredService<TContext>();
        await Task.CompletedTask;
    }


    public TContext GetDbContext()
    {
        return _currenDbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _currenDbContext.SaveChangesAsync(cancellationToken);
    }

    public int SaveChanges()
    {
        return _currenDbContext.SaveChanges();
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
    {
        return _serviceProvider.GetService<IRepository<TEntity>>();
    }

    public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
    {
        return _serviceProvider.GetService<IRepository<TEntity, TKey>>();
    }


    public TRep GetCustomRepository<TRep>() where TRep : IRepository
    {
        return _serviceProvider.GetService<TRep>();
    }


    public async ValueTask DisposeAsync()
    {
        await _currenDbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~EFCoreUnitOfWork()
    // {
    //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: 释放托管状态(托管对象)
                _currenDbContext?.Dispose();
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }
}
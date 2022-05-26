using DncyTemplate.Domain.Infra;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DncyTemplate.Domain.UnitOfWork;

/// <summary>
/// 单元工作范围管理器
/// 需要新开子范围时使用，需要具体的unitofwork 订阅 OnScopeChanged 事件
/// </summary>
public class UnitOfWorkScopeManager
{
    private readonly AsyncLocal<IServiceProvider> _currentScope = new();
    private readonly IServiceProvider _serviceProvider;

    public UnitOfWorkScopeManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _currentScope.Value = serviceProvider;
    }

    public event Func<IServiceProvider, Task> OnScopeChanged;


    public IDisposable Begin()
    {
        IServiceProvider parentScope = _currentScope.Value;
        IServiceScope newScope = _serviceProvider.CreateScope();
        _currentScope.Value = newScope.ServiceProvider;
        OnScopeChanged?.Invoke(newScope.ServiceProvider);
        return new DisposeAction(() =>
        {
            _currentScope.Value = parentScope;
            OnScopeChanged?.Invoke(parentScope);
            newScope?.Dispose();
        });
    }

    public async Task CompleteAsync()
    {
        var uowOptions = _currentScope.Value?.GetService<IOptions<UnitOfWorkCollectionOptions>>()?.Value;
        if (uowOptions is not null && uowOptions?.DbContexts is { Count: > 0 })
        {
            foreach (KeyValuePair<string, Type> item in uowOptions?.DbContexts)
            {
                if (_currentScope.Value?.GetService(item.Value) is not IUnitOfWork uow)
                {
                    continue;
                }
                await uow?.SaveChangesAsync();
            }
        }
    }
}
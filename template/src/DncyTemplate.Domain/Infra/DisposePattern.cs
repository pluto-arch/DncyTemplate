namespace DncyTemplate.Domain.Infra;

/*
 * 释放器模式
 */

public class DisposeAction : IDisposable
{
    private readonly Action _action;

    public DisposeAction(Action action)
    {
        _action = action;
    }

    void IDisposable.Dispose()
    {
        _action();
        GC.SuppressFinalize(this);
    }
}


public class AsyncDisposeAction : IAsyncDisposable
{
    private readonly Func<Task> _action;

    public AsyncDisposeAction(Func<Task> action)
    {
        _action = action;
    }

    public async ValueTask DisposeAsync()
    {
        await _action();
        GC.SuppressFinalize(this);
    }
}

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

namespace DncyTemplate.Application.Command;

public interface ICommand
{
    /// <summary>
    /// 是否是事务性的
    /// </summary>
    public bool Transactional() => false;
}

public interface ICommand<T> : ICommand, IRequest<T>
{
}
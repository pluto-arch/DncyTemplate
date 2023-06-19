namespace DncyTemplate.Application.Command;

public interface ICommand
{
    /// <summary>
    /// 是否是事务性的
    /// </summary>
    bool Transactional { get; }
}

public interface ICommand<T> : ICommand, IRequest<T>
{
}
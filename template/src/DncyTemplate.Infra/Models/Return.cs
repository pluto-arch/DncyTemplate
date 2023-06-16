namespace DncyTemplate;

/// <summary>
/// 方法返回值包装器
/// </summary>
/// <remarks>避免方法执行尽用抛异常方式进行错误返回</remarks>
/// <typeparam name="T">返回数据类型</typeparam>
/// <typeparam name="E">错误信息类型</typeparam>
public record Return<T, E>
{
    private readonly bool _success;
    private readonly T value;
    private readonly E error;

    private Return(T v, E e, bool success)
    {
        value = v;
        error = e;
        _success = success;
    }


    public bool Successed => _success;


    public T Data => value;

    public E Errors => error;


    public static Return<T, E> Success(T v)
    {
        return new(v, default(E), true);
    }

    public static Return<T, E> Error(E e)
    {
        return new(default(T), e, false);
    }


    public static implicit operator Return<T, E>(T v) => new(v, default(E), true);
    public static implicit operator Return<T, E>(E e) => new(default(T), e, false);

    public R Match<R>(Func<T, R> success, Func<E, R> failure)
        => _success ? success(value) : failure(error);
}
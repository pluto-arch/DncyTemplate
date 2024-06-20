

namespace DncyTemplate.Application.Models;


/// <summary>
/// api 结果包装空接口
/// </summary>
public interface IResponseWraps { }


/// <summary>
/// api返回包装结构
/// </summary>
public record ResultDto
{
    /// <summary>
    /// 接口是否处理成功
    /// </summary>
    public bool Successed { get; set; }

    /// <summary>
    /// 业务错误码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string Message { get; set; }


    /// <summary>
    ///     成功 - 空返回值
    /// </summary>
    /// <returns></returns>
    public static ResultDto Success()
    {
        return new() { Code = 200, Message = "Successed", Successed = true };
    }

    /// <summary>
    ///     业务错误
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ResultDto Error(string message)
    {
        return new() { Code = -100, Message = message };
    }

    /// <summary>
    /// 请求不合法
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ResultDto ErrorRequest(string message = "InvalidRequest")
    {
        return new() { Code = 400, Message = message };
    }

    /// <summary>
    /// 程序错误
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ResultDto Fatal(string message = "ServiceUnavailable")
    {
        return new() { Code = 500, Message = message };
    }

    /// <summary>
    /// 程序错误
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ResultDto TooManyRequest(string message = "TooManyRequest")
    {
        return new() { Code = 429, Message = message };
    }
}

/// <summary>
/// api返回包装结构 - 泛型
/// </summary>
public record ResultDto<T> : ResultDto
{
    public T Data { get; set; }

    /// <summary>
    ///     执行成功
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static ResultDto<T> Success(T data)
    {
        return new() { Code = 200, Message = "Successed", Data = data, Successed = true };
    }


    /// <summary>
    ///     执行成功
    /// </summary>
    /// <returns></returns>
    public static ResultDto<T> Success(T data, string message = "Successed")
    {
        return new() { Code = 200, Message = message, Data = data, Successed = true };
    }

    /// <summary>
    ///     业务错误
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static ResultDto<T> Error(string message = "ErrorHandleRequest", T data = default)
    {
        return new() { Code = -100, Message = message, Data = data };
    }

    /// <summary>
    /// 程序错误
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static ResultDto<T> Fatal(string message = "ServiceUnavailable", T data = default)
    {
        return new() { Code = 500, Message = message, Data = data };
    }

    /// <summary>
    /// 数据验证错误 无效的请求
    /// </summary>
    /// <returns></returns>
    public static ResultDto<T> ErrorRequest(string message = "InvalidRequest", T data = default)
    {
        return new() { Code = 400, Message = message, Data = data };
    }


    public static implicit operator ResultDto<T>(T v) => ResultDto<T>.Success(v);

    public static implicit operator ResultDto<T>(string error) => ResultDto<T>.Error(error);

}


/// <summary>
/// api结果包装扩展
/// </summary>
public static class ResponseWapper
{
    public static ResultDto<TData> Success<TData>(this IResponseWraps _, TData result)
    {
        return ResultDto<TData>.Success(result);
    }

    public static ResultDto<TData> Success<TData>(this IResponseWraps _, TData result, string message = "Successed")
    {
        return ResultDto<TData>.Success(result, message);
    }

    public static ResultDto<TData> Fail<TData>(this IResponseWraps _, string message = "ServiceUnavailable", TData data = default)
    {
        return ResultDto<TData>.Fatal(message, data);
    }
    public static ResultDto<TData> Error<TData>(this IResponseWraps _, string message = "ErrorHandleRequest", TData data = default)
    {
        return ResultDto<TData>.Error(message, data);
    }
    public static ResultDto<TData> ErrorRequest<TData>(this IResponseWraps _, string message = "InvalidRequest", TData data = default)
    {
        return ResultDto<TData>.ErrorRequest(message, data);
    }

    public static ResultDto Success(this IResponseWraps _)
    {
        return ResultDto.Success();
    }
    public static ResultDto ErrorRequest(this IResponseWraps _, string message = "InvalidRequest")
    {
        return ResultDto.ErrorRequest(message);
    }
    public static ResultDto Error(this IResponseWraps _, string message = "ErrorHandleRequest")
    {
        return ResultDto.Error(message);
    }
    public static ResultDto Fail(this IResponseWraps _, string message = "ServiceUnavailable")
    {
        return ResultDto.Fatal(message);
    }
}



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
    public int Code { get; set; }

    public string Message { get; set; }


    /// <summary>
    ///     成功 - 空返回值
    /// </summary>
    /// <returns></returns>
    public static ResultDto Success()
    {
        return new() { Code = 200, Message = "执行成功" };
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
    public static ResultDto ErrorRequest(string message = "无效的请求")
    {
        return new() { Code = 400, Message = message };
    }

    /// <summary>
    /// 程序错误
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ResultDto Fatal(string message = "内部服务器异常")
    {
        return new() { Code = 500, Message = message ?? "内部服务器异常" };
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
        return new() { Code = 200, Message = "执行成功", Data = data };
    }


    /// <summary>
    ///     执行成功
    /// </summary>
    /// <returns></returns>
    public static ResultDto<T> Success(T data, string message)
    {
        return new() { Code = 200, Message = message, Data = data };
    }

    /// <summary>
    ///     业务错误
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static ResultDto<T> Error(string message, T data = default)
    {
        return new() { Code = -100, Message = message, Data = data };
    }

    /// <summary>
    ///     程序错误
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static ResultDto<T> Fatal(string message, T data = default)
    {
        return new() { Code = 500, Message = message, Data = data };
    }

    /// <summary>
    ///     数据验证错误
    /// </summary>
    /// <returns></returns>
    public static ResultDto<T> ErrorRequest(string message = "无效的请求", T data = default)
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

    public static ResultDto<TData> Success<TData>(this IResponseWraps _, TData result, string message)
    {
        return ResultDto<TData>.Success(result, message);
    }

    public static ResultDto<TData> Fail<TData>(this IResponseWraps _, string message = "服务异常", TData data = default)
    {
        return ResultDto<TData>.Fatal(message, data);
    }
    public static ResultDto<TData> Error<TData>(this IResponseWraps _, string message = "处理请求出现错误", TData data = default)
    {
        return ResultDto<TData>.Error(message, data);
    }
    public static ResultDto<TData> ErrorRequest<TData>(this IResponseWraps _, string message = "无效的请求", TData data = default)
    {
        return ResultDto<TData>.ErrorRequest(message, data);
    }

    public static ResultDto Success(this IResponseWraps _)
    {
        return ResultDto.Success();
    }
    public static ResultDto ErrorRequest(this IResponseWraps _, string message = "无效的请求")
    {
        return ResultDto.ErrorRequest(message);
    }
    public static ResultDto Error(this IResponseWraps _, string message = "处理请求出现错误")
    {
        return ResultDto.Error(message);
    }
    public static ResultDto Fail(this IResponseWraps _, string message = "服务异常")
    {
        return ResultDto.Fatal(message);
    }
}

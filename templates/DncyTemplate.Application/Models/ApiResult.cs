namespace DncyTemplate.Application.Models;

public class ApiResult
{
    public int Code { get; set; }

    public string Message { get; set; }


    /// <summary>
    ///     成功 - 空返回值
    /// </summary>
    /// <returns></returns>
    public static ApiResult Success()
    {
        return new() { Code = 200, Message = "执行成功" };
    }


    /// <summary>
    ///     业务错误
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ApiResult Error(string message)
    {
        return new() { Code = -100, Message = message };
    }

    /// <summary>
    ///     业务错误
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ApiResult RequestError(string message = "无效的请求")
    {
        return new() { Code = 400, Message = message };
    }


    public static ApiResult InternalServerError(string message = "内部服务器异常")
    {
        return new() { Code = 500, Message = message ?? "内部服务器异常" };
    }
}


public class ApiResult<T> : ApiResult
{
    public T Data { get; set; }

    /// <summary>
    ///     执行成功
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static ApiResult<T> Success(T data)
    {
        return new() { Code = 200, Message = "执行成功", Data = data };
    }

    /// <summary>
    ///     业务错误
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static ApiResult<T> Error(string message, T data = default)
    {
        return new() { Code = -100, Message = message, Data = data };
    }

    /// <summary>
    ///     程序错误
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static ApiResult<T> Fatal(string message, T data = default)
    {
        return new() { Code = 500, Message = message, Data = data };
    }

    /// <summary>
    ///     数据验证错误
    /// </summary>
    /// <returns></returns>
    public static ApiResult<T> RequestError(string message = "无效的请求", T data = default)
    {
        return new() { Code = 400, Message = message, Data = data };
    }
}
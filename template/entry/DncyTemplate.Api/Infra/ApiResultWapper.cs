namespace DncyTemplate.Api.Infra;


/// <summary>
/// api 结果包装空接口
/// </summary>
public interface IApiResultWapper { }


/// <summary>
/// api返回包装结构
/// </summary>
public record ApiResult
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
    /// 请求不合法
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ApiResult ErrorRequest(string message = "无效的请求")
    {
        return new() { Code = 400, Message = message };
    }

    /// <summary>
    /// 程序错误
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ApiResult Fatal(string message = "内部服务器异常")
    {
        return new() { Code = 500, Message = message ?? "内部服务器异常" };
    }
}

/// <summary>
/// api返回包装结构 - 泛型
/// </summary>
public record ApiResult<T> : ApiResult
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
    ///     执行成功
    /// </summary>
    /// <returns></returns>
    public static ApiResult<T> Success(T data, string message)
    {
        return new() { Code = 200, Message = message, Data = data };
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
    public static ApiResult<T> ErrorRequest(string message = "无效的请求", T data = default)
    {
        return new() { Code = 400, Message = message, Data = data };
    }
}


/// <summary>
/// api结果包装扩展
/// </summary>
public static class ApiResultWapper
{
    public static ApiResult<TData> Success<TData>(this IApiResultWapper _, TData result)
    {
        return ApiResult<TData>.Success(result);
    }

    public static ApiResult<TData> Success<TData>(this IApiResultWapper _, TData result, string message)
    {
        return ApiResult<TData>.Success(result, message);
    }

    public static ApiResult<TData> Fail<TData>(this IApiResultWapper _, string message = "服务异常", TData data = default)
    {
        return ApiResult<TData>.Fatal(message, data);
    }
    public static ApiResult<TData> Error<TData>(this IApiResultWapper _, string message = "处理请求出现错误", TData data = default)
    {
        return ApiResult<TData>.Error(message, data);
    }
    public static ApiResult<TData> ErrorRequest<TData>(this IApiResultWapper _, string message = "无效的请求", TData data = default)
    {
        return ApiResult<TData>.ErrorRequest(message, data);
    }

    public static ApiResult Success(this IApiResultWapper _)
    {
        return ApiResult.Success();
    }
    public static ApiResult ErrorRequest(this IApiResultWapper _, string message = "无效的请求")
    {
        return ApiResult.ErrorRequest(message);
    }
    public static ApiResult Error(this IApiResultWapper _, string message = "处理请求出现错误")
    {
        return ApiResult.Error(message);
    }
    public static ApiResult Fail(this IApiResultWapper _, string message = "服务异常")
    {
        return ApiResult.Fatal(message);
    }
}

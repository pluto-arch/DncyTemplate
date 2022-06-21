using DncyTemplate.Application.Models;

namespace DncyTemplate.Api.Controllers;

public static class WrapperResultActionResultExtension
{
    public static ApiResult<TData> Success<TData>(this IWrapperResult _, TData result)
    {
        return ApiResult<TData>.Success(result);
    }

    public static ApiResult<TData> Fail<TData>(this IWrapperResult _, string message = "服务异常", TData data = default)
    {
        return ApiResult<TData>.Fatal(message, data);
    }
    public static ApiResult<TData> Error<TData>(this IWrapperResult _, string message = "处理失败", TData data = default)
    {
        return ApiResult<TData>.Error(message, data);
    }
    public static ApiResult<TData> RequestError<TData>(this IWrapperResult _, string message = "请求异常", TData data = default)
    {
        return ApiResult<TData>.RequestError(message, data);
    }
    public static ApiResult RequestError(this IWrapperResult _, string message = "请求异常")
    {
        return ApiResult.RequestError(message);
    }
    public static ApiResult Success(this IWrapperResult _)
    {
        return ApiResult.Success();
    }
}
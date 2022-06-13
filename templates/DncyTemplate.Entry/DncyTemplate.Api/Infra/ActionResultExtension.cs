using DncyTemplate.Application.Models;

namespace DncyTemplate.Api.Controllers;

public static class ActionResultExtension
{
    [NonAction]
    public static ApiResult<TData> Success<TData>(this ControllerBase _,TData result)
    {
        return ApiResult<TData>.Success(result);
    }

    [NonAction]
    public static ApiResult<TData> Fail<TData>(this ControllerBase _,string message = "服务异常", TData data = default)
    {
        return ApiResult<TData>.Fatal(message, data);
    }
    [NonAction]
    public static ApiResult<TData> Error<TData>(this ControllerBase _,string message = "处理失败", TData data = default)
    {
        return ApiResult<TData>.Error(message, data);
    }
    [NonAction]
    public static ApiResult<TData> RequestError<TData>(this ControllerBase _,string message = "请求异常", TData data = default)
    {
        return ApiResult<TData>.RequestError(message, data);
    }
    [NonAction]
    public static ApiResult RequestError(this ControllerBase _,string message = "请求异常")
    {
        return ApiResult.RequestError(message);
    }
    [NonAction]
    public static ApiResult Success(this ControllerBase _)
    {
        return ApiResult.Success();
    }
}
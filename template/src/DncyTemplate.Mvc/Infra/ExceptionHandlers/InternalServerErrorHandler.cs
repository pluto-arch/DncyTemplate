using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using System.Web;

namespace DncyTemplate.Mvc.Infra.ExceptionHandlers;

public static class InternalServerErrorHandler
{
    public static readonly RequestDelegate Handler = async context =>
    {
        var log = context.RequestServices.GetService<ILogger<Program>>() ?? NullLogger<Program>.Instance;
        var code = context.Response.StatusCode;
        var originalPath = context.Request.Path;
        var originalQueryString = context.Request.QueryString;
        var originalPathAndQuery = string.Join(context.Request.PathBase.Value ?? string.Empty,
            originalPath.Value ?? string.Empty,
            originalQueryString.HasValue ? originalQueryString.Value : null);

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error is not null)
        {
            log.LogError(exceptionHandlerPathFeature.Error, "应用服务出现异常：{msg}", exceptionHandlerPathFeature.Error.Message);
        }
        var newPath = new PathString($"/error/{code}/{HttpUtility.UrlEncode(originalPathAndQuery)}");
        context.Response.Redirect(newPath);
        await Task.CompletedTask;
    };
}
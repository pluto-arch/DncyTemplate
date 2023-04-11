using DncyTemplate.Application.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;

namespace DncyTemplate.Api.Infra.ExceptionHandlers;

public class HttpPipelineExceptionHandler
{
    public static readonly RequestDelegate Handler = async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = AppConstant.DEFAULT_CONTENT_TYPE;
        var res = ResultDto.Fatal();
        var log = context.RequestServices.GetService<ILogger<HttpPipelineExceptionHandler>>() ?? NullLogger<HttpPipelineExceptionHandler>.Instance;
        var options = context.RequestServices.GetRequiredService<IOptions<MvcNewtonsoftJsonOptions>>();
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error is not null)
        {
            log.LogError(exceptionHandlerPathFeature.Error, "应用服务出现异常：{msg}", exceptionHandlerPathFeature.Error.Message);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(res, options.Value.SerializerSettings));
        }
    };
}
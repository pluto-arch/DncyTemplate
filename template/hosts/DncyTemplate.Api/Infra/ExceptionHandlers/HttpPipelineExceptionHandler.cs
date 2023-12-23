using DncyTemplate.Application.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;

namespace DncyTemplate.Api.Infra.ExceptionHandlers
{
    public class HttpPipelineExceptionHandler
    {
        public static readonly RequestDelegate Handler = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = AppConstant.DEFAULT_CONTENT_TYPE;
            var log = context.RequestServices.GetService<ILogger<HttpPipelineExceptionHandler>>() ?? NullLogger<HttpPipelineExceptionHandler>.Instance;
            var l = context.RequestServices.GetService<IStringLocalizer<SharedResources>>();
            var options = context.RequestServices.GetRequiredService<IOptions<MvcNewtonsoftJsonOptions>>();
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature?.Error is not null)
            {
                var res = ResultDto<string>.Fatal(data: l.GetString("trace id: {0}", context.TraceIdentifier));
                res.Message = $"{l[res.Message]}: {exceptionHandlerPathFeature.Error.Message}";
                log.LogError(exceptionHandlerPathFeature.Error, "应用服务出现异常：{msg}", exceptionHandlerPathFeature.Error.Message);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(res, options.Value.SerializerSettings));
            }
        };
    }
}
using DncyTemplate.Application.Models;
using Microsoft.AspNetCore.Diagnostics;
using Polly;
using Serilog;
using Serilog.Context;

namespace DncyTemplate.ApiEndpoint.Infra
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 请求日志
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseHttpRequestLogging(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging(config =>
            {
                
                config.EnrichDiagnosticContext = (context, httpContext) =>
                {
                    if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                        context.Set("x_forwarded_for", httpContext.Request.Headers["X-Forwarded-For"]);
                    context.Set("request_path", httpContext.Request.Path);
                    context.Set("request_method", httpContext.Request.Method);
                };
                config.IncludeQueryInRequestPath = true;
            });
            return app;
        }



        /// <summary>
        /// 响应自动添加traceid
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UserResponseHeaderAuthTraceId(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var activityId = Activity.Current?.Id;
                var traceId = context.TraceIdentifier;

                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey("x-trace-id"))
                    {
                        context.Response.Headers.Append("x-trace-id", traceId??"");
                    }
                    if (!context.Response.Headers.ContainsKey("x-activity-id"))
                    {
                        context.Response.Headers.Append("x-activity-id", activityId);
                    }
                    return Task.CompletedTask;
                });
                await next(context);
            });
            return app;
        }


        /// <summary>
        /// 异常处理中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomizeExceptionHandle(this IApplicationBuilder app)
        {
            // 使用problemDetails
            app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                {
                    var mathErrorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (mathErrorFeature is not null)
                    {
                        var detail = mathErrorFeature.Error.Message;
                        await context.Response.WriteAsJsonAsync(ResultDto.Fatal($"An error occurred : {detail}"));
                        return;
                    }
                    await context.Response.WriteAsJsonAsync(ResultDto.Fatal("an error occurred"));
                });
            });
            return app;
        }
    }
}

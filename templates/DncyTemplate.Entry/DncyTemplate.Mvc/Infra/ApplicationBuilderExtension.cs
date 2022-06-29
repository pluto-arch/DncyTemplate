using DncyTemplate.Mvc.Infra.ExceptionHandlers;

namespace DncyTemplate.Mvc.Infra;

public static class ApplicationBuilderExtension
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
        });
        return app;
    }


    /// <summary>
    /// 异常处理中间件
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseInternalServerErrorHandle(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(InternalServerErrorHandler.Handler);
        });
        return app;
    }
}
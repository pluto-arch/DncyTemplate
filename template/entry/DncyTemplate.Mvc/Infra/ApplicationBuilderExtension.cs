﻿using DncyTemplate.Domain.Infra;
using DncyTemplate.Infra.Global;
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


    /// <summary>
    /// 用户访问器
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCurrentUserAccessor(this IApplicationBuilder app)
    {
        // 用户访问器
        app.Use((context, next) =>
        {
            var currentToken = context.RequestServices.GetService<GlobalAccessor.CurrentUser>();
            using (currentToken?.Change(context.User))
            {
                return next();
            }
        });
        return app;
    }


    /// <summary>
    /// 种子数据初始化
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static async Task<IApplicationBuilder> DataSeederAsync(this IApplicationBuilder app)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
        var seeders = serviceScope.ServiceProvider.GetServices<IDataSeedProvider>();
        if (!seeders.Any())
        {
            return app;
        }

        foreach (var seeder in seeders.OrderByDescending(x => x.Sorts).ToList())
        {
            await seeder.SeedAsync(serviceScope.ServiceProvider);
        }

        return app;
    }
}
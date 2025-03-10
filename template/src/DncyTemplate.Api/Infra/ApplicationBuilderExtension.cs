﻿using DncyTemplate.Api.Infra.ExceptionHandlers;
using DncyTemplate.Infra.Global;

namespace DncyTemplate.Api.Infra
{
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
                config.IncludeQueryInRequestPath = true;
            });
            return app;
        }

        /// <summary>
        /// 异常处理中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomExceptionHandle(this IApplicationBuilder app)
        {
            // 使用problemDetails
            // app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.Run(async context => await Results.Problem().ExecuteAsync(context)));


            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                // 使用自定义异常处理
                exceptionHandlerApp.Run(HttpPipelineExceptionHandler.Handler);
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
    }
}
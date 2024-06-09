using Asp.Versioning.ApiExplorer;
using DncyTemplate.Application.Models;
using DncyTemplate.Infra.Global;
using Microsoft.AspNetCore.Diagnostics;

namespace DncyTemplate.Api.Infra
{
    public static class ApplicationBuilderExtension
    {

        /// <summary>
        /// Swagger
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            var versionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseSwaggerUI(options =>
            {
                // 自定义ui使用
                //options.IndexStream = () => typeof(Program).Assembly.GetManifestResourceStream("DncyTemplate.Api.Infra.ApiDoc.index.html");


                foreach (var description in versionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"{AppConstant.SERVICE_NAME} - {description.GroupName}");
                }
            });
            return app;
        }


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


        /// <summary>
        /// 响应自动添加traceid
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UserResponseHeaderAuthTraceId(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey("x-trace-id"))
                    {
                        context.Response.Headers.Append("x-trace-id", context.TraceIdentifier ?? Activity.Current?.Id);
                    }
                    if (!context.Response.Headers.ContainsKey("x-activity-id"))
                    {
                        context.Response.Headers.Append("x-activity-id", Activity.Current?.Id);
                    }
                    return Task.CompletedTask;
                });
                await next(context);
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
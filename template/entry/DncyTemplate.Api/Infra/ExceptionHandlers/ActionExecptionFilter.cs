using DncyTemplate.Application.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging.Abstractions;

namespace DncyTemplate.Api.Infra.ExceptionHandlers
{
    public class ActionExecptionFilter : IAsyncExceptionFilter
    {

        private static readonly MediaTypeCollection mediaType = new()
        {
            AppConstant.DEFAULT_CONTENT_TYPE
        };

        /// <inheritdoc />
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                var log = context.HttpContext.RequestServices.GetService<ILogger<ActionExecptionFilter>>() ?? NullLogger<ActionExecptionFilter>.Instance;
                var msg = context.Exception.Message;
                log.LogError(context.Exception, "处理{method} {path}. 出现错误: {msg}", context.HttpContext.Request.Method, context.HttpContext.Request.GetEncodedPathAndQuery(), msg);
                context.Result = new ObjectResult(ResultDto.Error("处理请求失败"))
                {
                    ContentTypes = mediaType,
                    StatusCode = StatusCodes.Status200OK
                };
            }
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}

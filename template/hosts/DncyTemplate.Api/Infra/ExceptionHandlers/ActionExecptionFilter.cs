using DncyTemplate.Application.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Localization;
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
                var l = context.HttpContext.RequestServices.GetService<IStringLocalizer<SharedResource>>();
                var error = ResultDto<string>.Error();
                error.Message = $"{l[error.Message]}: {msg}";
                context.Result = new ObjectResult(error)
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

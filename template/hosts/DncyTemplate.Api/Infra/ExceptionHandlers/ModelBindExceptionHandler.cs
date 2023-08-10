using DncyTemplate.Application.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace DncyTemplate.Api.Infra.ExceptionHandlers
{
    public class ModelBindExceptionHandler
    {
        public static readonly Func<ActionContext, IActionResult> Handler = actionContext =>
        {
            var log = actionContext.HttpContext?.RequestServices?.GetService<ILogger<ModelBindExceptionHandler>>() ?? NullLogger<ModelBindExceptionHandler>.Instance;
            var result = new BadRequestObjectResult(actionContext.ModelState);
            result.ContentTypes.Add(AppConstant.DEFAULT_CONTENT_TYPE);
            log.LogWarning("{method} {@route} 模型绑定失败： {@msg}", actionContext.HttpContext.Request.Method, actionContext.RouteData.Values, result.Value);
            result.Value = ResultDto<object>.ErrorRequest(data: result.Value);
            return result;
        };
    }
}
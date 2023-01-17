using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DncyTemplate.Mvc.Infra.AuditLog;

public class AuditLogActionFilter : IAsyncActionFilter
{
    private readonly ILogger<AuditLogActionFilter> _logger;

    public AuditLogActionFilter(ILogger<AuditLogActionFilter> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!ShouldSaveAudit(context))
        {
            await next();
            return;
        }

        var type = (context.ActionDescriptor as ControllerActionDescriptor).ControllerTypeInfo.AsType();

        //方法信息
        var method = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo;
        //方法参数
        var arguments = context.ActionArguments;
        // var ip=context.HttpContext.Connection.RemoteIpAddress.ToString();
        // var trueIp = request.Headers["CF-Connecting-IP"].ToString(); // 由于X-Forwarded-For标头可能会被客户端篡改进行伪造，可以用CDN提供的一个转发客户端真实IP并且不可篡改的标头进行双重检查
        var auditInfo = new ApiAuditInfoModel
        {
            UserInfo = context.HttpContext.User?.Identity?.Name??"",
            ServiceName = type.FullName,
            MethodName = method.Name,
            ////请求参数转Json
            Parameters = JsonConvert.SerializeObject(arguments),
            ExecutionTime = DateTime.Now,
            BrowserInfo = context.HttpContext.Request.Headers["User-Agent"].ToString(),
            ClientIpAddress = context.HttpContext.Connection.RemoteIpAddress.ToString(),
        };

        //开始计时
        var stopwatch = Stopwatch.StartNew();

        ActionExecutedContext result = null;
        try
        {
            result = await next();

            if (result.Exception != null && !result.ExceptionHandled)
            {
                // 错误信息字段
                auditInfo.Exception = result.Exception;
            }
        }
        catch (Exception ex)
        {
            // 错误信息字段
            auditInfo.Exception = ex;
            throw;
        }
        finally
        {
            stopwatch.Stop();
            // 执行时长

            if (result is {Result: not ViewResult}) // 结果信息
            {
                auditInfo.ReturnValue = result.Result switch
                {
                    ObjectResult objectResult => JsonConvert.SerializeObject(objectResult.Value),
                    JsonResult jsonResult => JsonConvert.SerializeObject(jsonResult.Value),
                    ContentResult contentResult => contentResult.Content,
                    _ => auditInfo.ReturnValue
                };
            }

            // 存储， 推荐使用eventbus发布，由消费这慢慢写入持久化设施
            _logger.LogInformation("audit log : {@info}", auditInfo);
        }
    }



    /// <summary>
    /// 是否需要记录审计
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private bool ShouldSaveAudit(ActionExecutingContext context)
    {
        if (!(context.ActionDescriptor is ControllerActionDescriptor descriptor))
            return false;
        var methodInfo = descriptor.MethodInfo;

        if (!methodInfo.IsPublic)
        {
            return false;
        }

        //if (methodInfo.GetCustomAttribute<AuditedAttribute>() != null)
        //{
        //    return true;
        //}

        //if (methodInfo.GetCustomAttribute<DisableAuditingAttribute>() != null)
        //{
        //    return false;
        //}

        //var classType = methodInfo.DeclaringType;
        //if (classType != null)
        //{
        //    if (classType.GetTypeInfo().GetCustomAttribute<AuditedAttribute>() != null)
        //    {
        //        return true;
        //    }

        //    if (classType.GetTypeInfo().GetCustomAttribute<AuditedAttribute>() != null)
        //    {
        //        return false;
        //    }
        //}
        return true;
    }

    public class MyJsonConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = value.GetType().GetProperties();
            foreach (var prop in t)
            {
                // filter secret value
            }

        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace DncyTemplate.Api.Infra.LogSetup;


/// <summary>
/// 接口审计日志
/// </summary>
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

        var type = ( context.ActionDescriptor as ControllerActionDescriptor ).ControllerTypeInfo.AsType();

        //方法信息
        var method = ( context.ActionDescriptor as ControllerActionDescriptor ).MethodInfo;
        //方法参数
        var arguments = context.ActionArguments;
        // var ip=context.HttpContext.Connection.RemoteIpAddress.ToString();
        // var trueIp = request.Headers["CF-Connecting-IP"].ToString(); // 由于X-Forwarded-For标头可能会被客户端篡改进行伪造，可以用CDN提供的一个转发客户端真实IP并且不可篡改的标头进行双重检查
        var auditInfo = new ApiAuditInfoModel
        {
            UserInfo = context.HttpContext.User?.Identity?.Name ?? "",
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

            if (result != null) // 结果信息
            {
                switch (result.Result)
                {
                    case ObjectResult objectResult:
                        auditInfo.ReturnValue = JsonConvert.SerializeObject(objectResult.Value);
                        break;

                    case JsonResult jsonResult:
                        auditInfo.ReturnValue = JsonConvert.SerializeObject(jsonResult.Value);
                        break;

                    case ContentResult contentResult:
                        auditInfo.ReturnValue = contentResult.Content;
                        break;
                }
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
        if (!( context.ActionDescriptor is ControllerActionDescriptor descriptor ))
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
}
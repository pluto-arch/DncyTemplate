using System.Reflection;
using DncyTemplate.Domain.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace DncyTemplate.Mvc.Infra.UnitofWork;

public class UowActionFilter:IAsyncActionFilter
{
    private readonly IOptions<UnitOfWorkCollectionOptions> _options;
    public UowActionFilter(IOptions<UnitOfWorkCollectionOptions> options)
    {
        _options = options;
    }


    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionDescriptor.IsControllerAction())
        {
            await next();
            return;
        }

        if (HasDisableUowAttr(context.ActionDescriptor.GetMethodInfo()))
        {
            await next();
            return;
        }

        var result = await next();
        if (result.Exception == null || result.ExceptionHandled)
        {
            var uowOptions = _options?.Value;
            if (uowOptions is not null && uowOptions?.DbContexts is { Count: > 0 })
            {
                foreach (KeyValuePair<string, Type> item in uowOptions?.DbContexts)
                {
                    if (context.HttpContext.RequestServices.GetService(item.Value) is not IUnitOfWork uow)
                    {
                        continue;
                    }
                    await uow?.SaveChangesAsync();
                }
            }
        }
    }


    public static bool HasDisableUowAttr(MethodInfo methodInfo)
    {
        var attrs = methodInfo.GetCustomAttributes(true).OfType<DisableUowAttribute>().ToArray();
        if (attrs.Length > 0)
        {
            return true;
        }

        attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<DisableUowAttribute>().ToArray();
        if (attrs.Length > 0)
        {
            return true;
        }

        return false;
    }
}
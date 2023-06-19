using DncyTemplate.Uow;

namespace DncyTemplate.Api.Infra.UnitOfWork;

public class UnitOfWorkMiddleware : IMiddleware 
{
    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        await next(context);

        var contexts=DataContextTypeCache.GetApplicationDataContextList();
        foreach (var item in contexts)
        {
            var ctx = context.RequestServices.GetService(item);
            if (ctx is IDataContext c && c.HasChanges())
            {
                await c.SaveChangesAsync(context.RequestAborted);
            }
        }
    }
}
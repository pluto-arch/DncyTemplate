﻿using DncyTemplate.Domain.UnitOfWork;

namespace DncyTemplate.Api.Infra.UnitofWork;

public class UnitOfWorkMiddleware
{
    private readonly RequestDelegate _next;

    public UnitOfWorkMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
        var uowOptions = context.RequestServices.GetService<IOptions<UnitOfWorkCollectionOptions>>()?.Value;
        if (uowOptions is not null && uowOptions?.DbContexts is { Count: > 0 })
        {
            foreach (KeyValuePair<string, Type> item in uowOptions?.DbContexts)
            {
                if (context.RequestServices.GetService(item.Value) is not IUnitOfWork uow)
                {
                    continue;
                }

                await uow?.SaveChangesAsync(context.RequestAborted);
            }
        }
    }
}
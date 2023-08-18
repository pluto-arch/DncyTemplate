#if Tenant
using Dncy.MultiTenancy;
#endif

using DncyTemplate.Domain.Infra;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DncyTemplate.Infra.EntityFrameworkCore.Interceptor;

public class DataChangeSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IDomainEventDispatcher _dispatcher;

    public DataChangeSaveChangesInterceptor(IDomainEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }


    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
           InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
#if Tenant
            MultiTenancyTracking(eventData.Context);
#endif
            SoftDeleteTracking(eventData.Context);
            DispatchDomainEventsAsync(eventData.Context).Wait();
        }
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
#if Tenant
            MultiTenancyTracking(eventData.Context);
#endif
            SoftDeleteTracking(eventData.Context);
            await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
        }
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void SoftDeleteTracking(DbContext dbContext)
    {
        var deletedEntries = dbContext.ChangeTracker.Entries()
            .Where(hosts => hosts.State == EntityState.Deleted && hosts.Entity is ISoftDelete);
        deletedEntries?.ToList().ForEach(entityEntry =>
        {
            entityEntry.Reload();
            entityEntry.State = EntityState.Modified;
            ( (ISoftDelete)entityEntry.Entity ).Deleted = true;
        });
    }

#if Tenant
    private static void MultiTenancyTracking(DbContext dbContext)
    {
        var tenantedEntries = dbContext.ChangeTracker.Entries<IMultiTenant>()
            .Where(hosts => hosts.State == EntityState.Added);
        var currentTenant = dbContext.GetService<ICurrentTenant>();
        tenantedEntries?.ToList().ForEach(entityEntry =>
        {
            entityEntry.Entity.TenantId ??= currentTenant.Id;
        });
    }
#endif


    private async Task DispatchDomainEventsAsync(DbContext dbContext, CancellationToken cancellationToken = default)
    {
        var domainEntities = dbContext.ChangeTracker.Entries<IDomainEvents>().Select(e => e.Entity);
        var domainEvents = domainEntities.SelectMany(x => x.DomainEvents).ToList();
        domainEntities.ToList().ForEach(entity => entity.ClearDomainEvents());
        foreach (var domainEvent in domainEvents)
        {
            await _dispatcher.Dispatch(domainEvent, cancellationToken);
        }
    }

}
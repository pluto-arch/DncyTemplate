using Dncy.MultiTenancy;
using DncyTemplate.Domain.Infra;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DncyTemplate.Infra.EntityFrameworkCore.Interceptor;

public class DataChangeSaveChangesInterceptor: SaveChangesInterceptor
{
    private readonly IDomainEventDispatcher _dispatcher;

    public DataChangeSaveChangesInterceptor(IDomainEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }


     public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            MultiTenancyTracking(eventData.Context);
            SoftDeleteTracking(eventData.Context);
            DispatchDomainEventsAsync(eventData.Context).Wait();
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            MultiTenancyTracking(eventData.Context);
            SoftDeleteTracking(eventData.Context);
            await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void SoftDeleteTracking(DbContext dbContext)
        {
            IEnumerable<EntityEntry> deletedEntries = dbContext.ChangeTracker.Entries()
                .Where(entry => entry.State == EntityState.Deleted && entry.Entity is ISoftDelete);
            deletedEntries?.ToList().ForEach(entityEntry =>
            {
                entityEntry.Reload();
                entityEntry.State = EntityState.Modified;
                ((ISoftDelete)entityEntry.Entity).Deleted = true;
            });
        }

        private static void MultiTenancyTracking(DbContext dbContext)
        {
            IEnumerable<EntityEntry<IMultiTenant>> tenantedEntries = dbContext.ChangeTracker.Entries<IMultiTenant>()
                .Where(entry => entry.State == EntityState.Added);
            ICurrentTenant currentTenant = dbContext.GetService<ICurrentTenant>();
            tenantedEntries?.ToList().ForEach(entityEntry =>
            {
                entityEntry.Entity.TenantId ??= currentTenant.Id;
            });
        }

        private async Task DispatchDomainEventsAsync(DbContext dbContext, CancellationToken cancellationToken = default)
        {
            IEnumerable<IDomainEvents> domainEntities =
                dbContext.ChangeTracker.Entries<IDomainEvents>().Select(e => e.Entity);
            List<INotification> domainEvents = domainEntities.SelectMany(x => x.DomainEvents).ToList();
            domainEntities.ToList().ForEach(entity => entity.ClearDomainEvents());
            foreach (INotification domainEvent in domainEvents)
            {
                await _dispatcher.Dispatch(domainEvent);
            }
        }

}
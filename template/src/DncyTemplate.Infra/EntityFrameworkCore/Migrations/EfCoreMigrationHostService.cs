using DncyTemplate.Constants;
using DncyTemplate.Domain.Infra;
using Dotnetydd.MultiTenancy;
using Dotnetydd.MultiTenancy.Model;
using Dotnetydd.Permission.Definition;
using Dotnetydd.Permission.PermissionGrant;
using Microsoft.Extensions.Hosting;

namespace DncyTemplate.Infra.EntityFrameworkCore.Migrations
{

    public class EfCoreMigrationHostService: IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHostEnvironment _env;
        private readonly ICurrentTenant _currentTenant;

        public EfCoreMigrationHostService(IServiceScopeFactory scopeFactory,IHostEnvironment env,ICurrentTenant currentTenant)
        {
            _scopeFactory = scopeFactory;
            _env = env;
            _currentTenant = currentTenant;
        }



        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope serviceScope = _scopeFactory.CreateScope();

            foreach (var dbcontext in DataContextTypeCache.GetApplicationDataContextList())
            {
                var db = serviceScope.ServiceProvider.GetService(dbcontext);
                if (db is DbContext ctx)
                {
                    await ctx.Database.MigrateAsync(cancellationToken);
                }
            }

#if Tenant
            var demotenant = new string[] { "T20210602000001", "T20210602000002", "T20210602000003" };
            foreach (var tenantId in demotenant)
            {
                using (_currentTenant.Change(new TenantInfo(tenantId)))
                {
                    await SeedSaPermissions(serviceScope.ServiceProvider);
                    await SeedData(serviceScope.ServiceProvider);
                }
            }
#else
            await SeedSaPermissions(serviceScope.ServiceProvider);
            await SeedData(serviceScope.ServiceProvider);
#endif

        }



        async Task SeedData(IServiceProvider service)
        {
            var seeders = service.GetServices<IDataSeedProvider>();
            if (!seeders.Any())
            {
                return;
            }
            foreach (var seeder in seeders.OrderByDescending(x => x.Sorts).ToList())
            {
                await seeder.SeedAsync(service);
            }
        }

        async Task SeedSaPermissions(IServiceProvider service)
        {
            var permissionManager = service.GetService<IPermissionDefinitionManager>();
            var permissionStore = service.GetService<IPermissionGrantStore>();

            var permission = permissionManager.GetPermissions().Select(x => x.Name);
            var saPermission = await permissionStore.GetListAsync(DomainConstantValue.PERMISSION_PROVIDER_NAME_ROLE, DomainConstantValue.SA_ROLE);
            if (!saPermission.Any())
            {
                await permissionStore.SaveAsync(permission.ToArray(), DomainConstantValue.PERMISSION_PROVIDER_NAME_ROLE, DomainConstantValue.SA_ROLE);
            }
            else
            {
                var ex = permission.Except(saPermission.Select(x => x.Name));
                if (ex.Any())
                {
                    await permissionStore.SaveAsync(ex.ToArray(), DomainConstantValue.PERMISSION_PROVIDER_NAME_ROLE, DomainConstantValue.SA_ROLE);
                }
            }

        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
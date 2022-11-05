using Dncy.MultiTenancy;
using Dncy.MultiTenancy.Model;
using Dncy.Permission;
using Dncy.Permission.Models;

using DncyTemplate.Domain.Aggregates.System;
using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Repository;
using DncyTemplate.Domain.UnitOfWork;

namespace DncyTemplate.Domain.Services.DataSeeds
{
    [Injectable(InjectLifeTime.Transient,typeof(IDataSeedProvider))]
    public class PermissionGrantDataSeedProvider : IDataSeedProvider
    {

        private readonly ICurrentTenant _currentTenant;
        private readonly IPermissionDefinitionManager _definitionManager;
        private readonly UnitOfWorkScopeManager _unitOfWorkScopeManager;

        public PermissionGrantDataSeedProvider(ICurrentTenant currentTenant, IPermissionDefinitionManager definitionManager,UnitOfWorkScopeManager unitOfWorkScopeManager)
        {
            _currentTenant = currentTenant;
            _definitionManager = definitionManager;
            _unitOfWorkScopeManager = unitOfWorkScopeManager;
        }
        public int Sorts => 100;


        /// <inheritdoc />
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            (string id, string name)[] tenantIds = new []
            {
                ("T20210602000001","租户一"),
                ("T20210602000002","租户二"),
                ("T20210602000003","租户三")
            };

            var per = _definitionManager.GetPermissions();


            var rep = serviceProvider.GetRequiredService<IRepository<PermissionGrant>>();
            TenantInfo t = null;
            foreach (var (id, name) in tenantIds)
            {
                using (_unitOfWorkScopeManager.Begin())
                using (_currentTenant.Change(new TenantInfo(id,name)))
                {
                    if (rep.Any())
                    {
                        continue;
                    }

                    foreach (var item in per)
                    {
                        await rep.InsertAsync(new PermissionGrant
                        {
                            Name = item.Name,
                            ProviderName = "role",
                            ProviderKey = "sa",
                        });
                    }
                    await rep.Uow.SaveChangesAsync();
                }
            }
        }
    }
}
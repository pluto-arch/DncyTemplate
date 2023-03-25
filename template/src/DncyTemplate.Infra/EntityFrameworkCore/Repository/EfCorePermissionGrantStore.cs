using Dncy.Permission;
using DncyTemplate.Domain.Aggregates.System;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;

namespace DncyTemplate.Infra.EntityFrameworkCore.Repository
{
    public class EfCorePermissionGrantStore : IPermissionGrantStore
    {
        private readonly IEfRepository<PermissionGrant> _permissionGrants;

        public EfCorePermissionGrantStore(EfUnitOfWork<DncyTemplateDbContext> uow)
        {
            _permissionGrants = uow.Repository<PermissionGrant>();
        }
        public async Task GrantAsync(string name, string providerName, string providerKey)
        {
            await _permissionGrants.InsertAsync(new PermissionGrant(name, providerName, providerKey), true);
        }

        public async Task GrantAsync(string[] name, string providerName, string providerKey)
        {
            var list= name.Select(x => new PermissionGrant(x,providerName,providerKey));
            await _permissionGrants.InsertAsync(list, true);
        }

        public async Task CancleGrantAsync(string name, string providerName, string providerKey)
        {
            await _permissionGrants.DeleteAsync(x => x.Name==name && x.ProviderKey == providerKey && x.ProviderName == providerName, true);
        }

        public async Task CancleGrantAsync(string[] name, string providerName, string providerKey)
        {
            await _permissionGrants.DeleteAsync(x=>name.Contains(x.Name) && x.ProviderKey == providerKey && x.ProviderName == providerName,true);
        }

        public async Task<IPermissionGrant> GetAsync(string name, string providerName, string providerKey)
        {
            return await _permissionGrants.FirstOrDefaultAsync(x=>x.Name==name&&x.ProviderKey==providerKey&&x.ProviderName==providerName);
        }

        public async Task<IEnumerable<IPermissionGrant>> GetListAsync(string providerName, string providerKey)
        {
            return await _permissionGrants.Where(x => x.ProviderKey == providerKey && x.ProviderName == providerName).ToListAsync();
        }

        public async Task<IEnumerable<IPermissionGrant>> GetListAsync(string[] names, string providerName, string providerKey)
        {
            return await _permissionGrants.Where(x => names.Contains(x.Name) && x.ProviderKey == providerKey && x.ProviderName == providerName).ToListAsync();
        }
    }
}

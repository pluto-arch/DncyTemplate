using DncyTemplate.Domain.Aggregates.System;
using DncyTemplate.Domain.Infra.Repository;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Uow;
using Dotnetydd.Permission.PermissionGrant;

namespace DncyTemplate.Infra.EntityFrameworkCore.Repository
{
    public class EfCorePermissionGrantStore(IUnitOfWork<DncyTemplateDbContext> uow) : IPermissionGrantStore
    {
        private readonly IEfRepository<PermissionGrant> _permissionGrants = uow.Resolve<IEfRepository<PermissionGrant>>();

        public async Task<IPermissionGrant> GetAsync(string name, string providerName, string providerKey)
        {
            return await _permissionGrants.FirstOrDefaultAsync(x => x.Name == name && x.ProviderKey == providerKey && x.ProviderName == providerName);
        }

        public async Task<IEnumerable<IPermissionGrant>> GetListAsync(string providerName, string providerKey)
        {
            return await _permissionGrants.Where(x => x.ProviderKey == providerKey && x.ProviderName == providerName).ToListAsync();
        }

        public async Task<IEnumerable<IPermissionGrant>> GetListAsync(string[] names, string providerName, string providerKey)
        {
            return await _permissionGrants.Where(x => names.Contains(x.Name) && x.ProviderKey == providerKey && x.ProviderName == providerName).ToListAsync();
        }

        public async Task SaveAsync(string name, string providerName, string providerKey)
        {
            await _permissionGrants.InsertAsync(new PermissionGrant(name, providerName, providerKey), true);
        }

        public async Task SaveAsync(string[] name, string providerName, string providerKey)
        {
            var list = name.Select(x => new PermissionGrant(x, providerName, providerKey));
            await _permissionGrants.InsertAsync(list, true);
        }

        public async Task RemoveGrantAsync(string name, string providerName, string providerKey)
        {
            await _permissionGrants.DeleteAsync(x => x.Name == name && x.ProviderKey == providerKey && x.ProviderName == providerName, true);
        }

        public async Task RemoveGrantAsync(string[] name, string providerName, string providerKey)
        {
            await _permissionGrants.DeleteAsync(x => name.Contains(x.Name) && x.ProviderKey == providerKey && x.ProviderName == providerName, true);
        }
    }
}

using Dncy.Permission;
using DncyTemplate.Domain.Aggregates.System;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DncyTemplate.Infra.EntityFrameworkCore.Repository
{
    public class EfCorePermissionGrantStore : IPermissionGrantStore
    {
        private readonly IEfGenericRepository<DncyTemplateDbContext, PermissionGrant> _permissionGrants;

        public EfCorePermissionGrantStore(EfUow<DncyTemplateDbContext> uow)
        {
            _permissionGrants = uow.Repository<PermissionGrant>();
        }


        /// <inheritdoc />
        public async Task<IPermissionGrant> GetAsync(string name, string providerName, string providerKey)
        {
            return await _permissionGrants.FirstOrDefaultAsync(s => s.ProviderName == providerName && s.ProviderKey == providerKey);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IPermissionGrant>> GetListAsync(string providerName, string providerKey)
        {
            return await _permissionGrants.AsNoTracking().Where(s => s.ProviderName == providerName && s.ProviderKey == providerKey)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IPermissionGrant>> GetListAsync(string[] names, string providerName, string providerKey)
        {
            return await _permissionGrants.AsNoTracking().Where(s => names.Contains(s.Name) && s.ProviderName == providerName && s.ProviderKey == providerKey)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task SaveAsync(string name, string providerName, string providerKey)
        {
            await _permissionGrants.InsertAsync(new PermissionGrant(name, providerName, providerKey), true);
        }

        /// <inheritdoc />
        public async Task SaveAsync(string[] name, string providerName, string providerKey)
        {
            var tempList = name.Select(x => new PermissionGrant(x, providerName, providerKey));
            await _permissionGrants.InsertAsync(tempList, true);
        }

        /// <inheritdoc />
        public async Task RemoveGrantAsync(string name, string providerName, string providerKey)
        {
            await _permissionGrants.DeleteAsync(x => x.Name == name && x.ProviderName == providerName && x.ProviderKey == providerKey, true);
        }

        /// <inheritdoc />
        public async Task RemoveGrantAsync(string[] name, string providerName, string providerKey)
        {
            await _permissionGrants.DeleteAsync(x => name.Contains(x.Name) && x.ProviderName == providerName && x.ProviderKey == providerKey, true);
        }
    }
}

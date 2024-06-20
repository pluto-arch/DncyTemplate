using DncyTemplate.Domain.Collections;
using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Infra.Repository;
using DncyTemplate.Uow;

namespace DncyTemplate.Infra.EntityFrameworkCore.Repository
{
    public class EfGenericRepository<TDbContext, TEntity> : IGenericRepository<TEntity>
        where TDbContext : DbContext, IDataContext
        where TEntity : class, IEntity
    {
        private readonly IUnitOfWork<TDbContext> _unitOfWork;

        public EfGenericRepository(IUnitOfWork<TDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        private TDbContext DbContext => _unitOfWork.Context as TDbContext;

        protected DbSet<TEntity> EntitySet => DbContext.Set<TEntity>();


        public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(EntitySet.AsNoTracking().AsEnumerable());
        }

        public virtual async Task<TEntity> GetAsync<TKey>(TKey key)
        {
            return await EntitySet.FindAsync([key]).AsTask();
        }


        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            var en = await EntitySet.AddAsync(entity).AsTask();
            return en.State == EntityState.Added ? 1 : 0;
        }

        public virtual async Task<int> InsertAsync(List<TEntity> entities)
        {
            await EntitySet.AddRangeAsync(entities);
            if (entities.Any(x => DbContext.Entry(x).State != EntityState.Added))
            {
                return 0;
            }
            return entities.Count;
        }

        public virtual Task<int> UpdateAsync(TEntity entity)
        {
            var en = EntitySet.Update(entity);
            return Task.FromResult(en.State == EntityState.Modified ? 1 : 0);
        }

        public virtual Task<int> UpdateAsync(List<TEntity> entities)
        {
            EntitySet.UpdateRange(entities);
            if (entities.Any(x => DbContext.Entry(x).State != EntityState.Modified))
            {
                return Task.FromResult(0);
            }
            return Task.FromResult(entities.Count);
        }

        public virtual Task<int> DeleteAsync(TEntity entity)
        {
            var en = EntitySet.Remove(entity);
            return Task.FromResult(en.State == EntityState.Deleted ? 1 : 0);
        }

        public virtual Task<int> DeleteAsync(List<TEntity> entities)
        {
            EntitySet.RemoveRange(entities);
            if (entities.Any(x => DbContext.Entry(x).State != EntityState.Deleted))
            {
                return Task.FromResult(0);
            }
            return Task.FromResult(entities.Count);
        }

        public virtual async Task<int> DeleteByIdAsync<TKey>(TKey key)
        {
            var en = await EntitySet.FindAsync([key]);
            return await DeleteAsync(en);
        }
    }
}
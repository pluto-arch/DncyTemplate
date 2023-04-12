using Dncy.Specifications;
using Dncy.Specifications.EntityFrameworkCore;
using Dncy.Specifications.EntityFrameworkCore.Evaluatiors;
using Dncy.Specifications.Evaluators;
using Dncy.Specifications.Exceptions;
using DncyTemplate.Domain.Collections;
using DncyTemplate.Domain.Exceptions;
using DncyTemplate.Domain.Infra;
using DncyTemplate.Infra.EntityFrameworkCore.Extension;
using System.Collections;
using System.Linq.Expressions;
using DncyTemplate.Domain.Infra.Repository;
using DncyTemplate.Domain.Infra.UnitOfWork;


namespace DncyTemplate.Infra.EntityFrameworkCore.Repository
{
    public class EfRepository<TContext, TEntity> : IEfRepository<TEntity>
         where TContext : DbContext,IDataContext
         where TEntity : class, IEntity
    {
        private readonly ISpecificationEvaluator _specification = EfCoreSpecificationEvaluator.Default;
        private readonly EfUnitOfWork<TContext> _unitOfWork;

        public EfRepository(EfUnitOfWork<TContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private TContext _dbContext => _unitOfWork.DbContext();
        protected DbSet<TEntity> _entitySet => _dbContext.Set<TEntity>();

        public virtual IQueryable<TEntity> QuerySet => _dbContext.Set<TEntity>();

        public IEnumerator<TEntity> GetEnumerator() => QuerySet.GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Type ElementType => QuerySet.ElementType;

        public Expression Expression => QuerySet.Expression;

        public IQueryProvider Provider => QuerySet.Provider;

        public TContext Uow => _dbContext;

        public virtual IAsyncQueryableProvider AsyncExecuter => new AsyncQueryableProvider();

        public virtual async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            TEntity savedEntity = _entitySet.Add(entity).Entity;
            if (autoSave)
                await _dbContext.SaveChangesAsync(cancellationToken);
            return savedEntity;
        }

        public virtual async Task InsertAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await _entitySet.AddRangeAsync(entities, cancellationToken);
            if (autoSave)
                await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _entitySet.Remove(entity);
            if (autoSave)
                await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false,
        CancellationToken cancellationToken = default)
        {
            var entities = QuerySet.Where(predicate);
            _entitySet.RemoveRange(entities);
            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task DeleteAsync(IEnumerable<TEntity> entities, bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            _entitySet.RemoveRange(entities);

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            _dbContext.Attach(entity);

            TEntity updatedEntity = _dbContext.Update(entity).Entity;

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return updatedEntity;
        }

        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities, bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            _entitySet.UpdateRange(entities);

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await QuerySet.CountAsync(cancellationToken);
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await QuerySet.CountAsync(predicate, cancellationToken);
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await ( await IncludeRelatedAsync() ).Where(predicate).SingleOrDefaultAsync(cancellationToken)
                : await QuerySet.Where(predicate).SingleOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await QuerySet.ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> sorting, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = includeDetails ? await IncludeRelatedAsync() : QuerySet;
            return await queryable.OrderBy(sorting).Where(predicate).ToListAsync(cancellationToken);
        }

        public virtual async Task<IPagedList<TEntity>> GetPageListAsync(int pageNo, int pageSize,
            Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> sorting,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = includeDetails ? await IncludeRelatedAsync() : QuerySet;
            return await queryable.OrderBy(sorting).Where(predicate)
                .ToPagedListAsync(pageNo, pageSize, cancellationToken);
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            TEntity entity = await FindAsync(predicate, includeDetails, cancellationToken);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity));
            }

            return entity;
        }

        public virtual async Task<List<TEntity>> GetListAsync(ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TResult>> GetListAsync<TResult>(ISpecification<TEntity, TResult> specification,
            CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).CountAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetAsync(ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default)
        {
            return ( await GetListAsync(specification, cancellationToken) ).FirstOrDefault()!;
        }

        public virtual async Task<TResult> GetAsync<TResult>(ISpecification<TEntity, TResult> specification,
            CancellationToken cancellationToken = default)
        {
            return ( await GetListAsync(specification, cancellationToken) ).FirstOrDefault()!;
        }

        /// <inheritdoc />
        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] param)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, param);
        }

        public virtual async Task<IQueryable<TEntity>> IncludeRelatedAsync(
            params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var includes = _dbContext.GetService<IOptions<IncludeRelatedPropertiesOptions>>().Value;

            IQueryable<TEntity> query = QuerySet;

            if (propertySelectors is not null)
            {
                propertySelectors.ToList().ForEach(propertySelector =>
                {
                    query = query.Include(propertySelector);
                });
            }
            else
            {
                query = includes.Get<TEntity>()(query);
            }

            return await Task.FromResult(query);
        }

        public virtual async Task LoadRelatedAsync<TProperty>(TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            CancellationToken cancellationToken = default) where TProperty : class
        {
            await _dbContext.Entry(entity).Collection(propertyExpression).LoadAsync(cancellationToken);
        }

        public virtual async Task LoadRelatedAsync<TProperty>(TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken = default) where TProperty : class
        {
            await _dbContext.Entry(entity).Reference(propertyExpression).LoadAsync(cancellationToken);
        }


        protected IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return _specification.GetQuery(QuerySet, specification);
        }

        protected IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification)
        {
            if (specification is null)
                ThrowHelper.ThrowArgumentNullException(nameof(specification));

            if (specification.Selector is null)
                throw new SelectorNotFoundException();

            return _specification.GetQuery(QuerySet, specification);
        }
    }


    public class EfRepository<TDbContext, TEntity, TKey> : EfRepository<TDbContext, TEntity>, IEfRepository<TEntity, TKey>
            where TDbContext : DbContext, IDataContext
            where TEntity : class, IEntity
    {
        public EfRepository(EfUnitOfWork<TDbContext> unitOfWork) : base(unitOfWork) { }

        public async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            TEntity entity = await FindAsync(id, cancellationToken);
            if (entity == null)
            {
                return;
            }

            await DeleteAsync(entity, autoSave, cancellationToken);
        }

        public async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            TEntity entity = await FindAsync(id, cancellationToken);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity));
            }

            return entity;
        }

        public async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await _entitySet.FindAsync(new object[] { id! }, cancellationToken);
        }
    }
}

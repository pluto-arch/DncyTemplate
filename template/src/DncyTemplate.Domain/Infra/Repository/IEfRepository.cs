using Dncy.Specifications;
using DncyTemplate.Domain.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DncyTemplate.Domain.Infra.Repository
{

    public interface IEfRepository<TEntity> : IQueryable<TEntity>
        where TEntity : class, IEntity
    {
        IQueryable<TEntity> QuerySet { get; }

        IAsyncQueryableProvider AsyncExecuter { get; }

        /// <summary>
        ///     Insert an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);


        /// <summary>
        ///     Insert entities
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task InsertAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);


        /// <summary>
        ///     删除一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        ///     按条件删除
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync([NotNull] Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        ///     删除多个
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);


        /// <summary>
        ///     更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        ///     更新
        /// </summary>
        /// <returns></returns>
        Task UpdateAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);


        /// <summary>
        ///     数量
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(CancellationToken cancellationToken = default);


        /// <summary>
        ///     数量
        /// </summary>
        /// <returns></returns>
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     find an entity
        /// </summary>
        /// <returns></returns>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     get list
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     获取列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="sorting"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> sorting, bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     获取列表
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="predicate"></param>
        /// <param name="sorting"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<TEntity>> GetPageListAsync(int pageNo, int pageSize, Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> sorting, bool includeDetails = false,
            CancellationToken cancellationToken = default);


        /// <summary>
        ///     查询单个
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync([NotNull] Expression<Func<TEntity, bool>> predicate, bool includeDetails = false,
            CancellationToken cancellationToken = default);


        #region Specification Method

        Task<List<TEntity>> GetListAsync(ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default);

        Task<List<TResult>> GetListAsync<TResult>(ISpecification<TEntity, TResult> specification,
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

        Task<TEntity> GetAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

        Task<TResult> GetAsync<TResult>(ISpecification<TEntity, TResult> specification,
            CancellationToken cancellationToken = default);

        #endregion


        #region Related Navigation Property

        Task<IQueryable<TEntity>> IncludeRelatedAsync(params Expression<Func<TEntity, object>>[] propertySelectors);

        Task LoadRelatedAsync<TProperty>(TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            CancellationToken cancellationToken = default) where TProperty : class;

        Task LoadRelatedAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken = default) where TProperty : class;

        #endregion


        /// <summary>
        /// 执行SQL语句返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlRawAsync(string sql, params object[] param);
    }


    public interface IEfRepository<TEntity, TKey> : IEfRepository<TEntity>
       where TEntity : class, IEntity
    {
        Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default);

        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);

        Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default);
    }


    /// <summary>
    /// 空接口
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEfContextRepository<TDbContext, TEntity> : IEfRepository<TEntity>
        where TEntity : class, IEntity
        where TDbContext : class
    {

    }

    public interface IEfContextRepository<TDbContext, TEntity, TKey> : IEfRepository<TEntity, TKey>
        where TEntity : class, IEntity
        where TDbContext : class
    {
    }
}

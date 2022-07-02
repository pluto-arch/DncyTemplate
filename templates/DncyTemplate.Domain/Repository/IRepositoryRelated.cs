using System.Linq.Expressions;

using DncyTemplate.Domain.Infra;

namespace DncyTemplate.Domain.Repository;

public partial interface IRepository<TEntity> where TEntity : IEntity
{
    #region Related Navigation Property

    Task<IQueryable<TEntity>> IncludeRelatedAsync(params Expression<Func<TEntity, object>>[] propertySelectors);

    Task LoadRelatedAsync<TProperty>(TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken = default) where TProperty : class;

    Task LoadRelatedAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression,
        CancellationToken cancellationToken = default) where TProperty : class;

    #endregion
}
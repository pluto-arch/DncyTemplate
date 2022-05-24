using System.Linq.Expressions;
using Dncy.Specifications.Builder;

namespace Dncy.Specifications.Extensions;

public static class IncludableBuilderExtensions
{
    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty,
        TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
        where TEntity : class
    {
        IncludeExpressionInfo info = new IncludeExpressionInfo(thenIncludeExpression, typeof(TEntity),
            typeof(TProperty), typeof(TPreviousProperty));

        ((List<IncludeExpressionInfo>)previousBuilder.Specification.IncludeExpressions).Add(info);

        IncludableSpecificationBuilder<TEntity, TProperty> includeBuilder =
            new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);

        return includeBuilder;
    }

    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty,
        TProperty>(
        this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
        where TEntity : class
    {
        IncludeExpressionInfo info = new IncludeExpressionInfo(thenIncludeExpression, typeof(TEntity),
            typeof(TProperty), typeof(TPreviousProperty));

        ((List<IncludeExpressionInfo>)previousBuilder.Specification.IncludeExpressions).Add(info);

        IncludableSpecificationBuilder<TEntity, TProperty> includeBuilder =
            new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);

        return includeBuilder;
    }
}
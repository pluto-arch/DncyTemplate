using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DncyTemplate.Infra.EntityFrameworkCore.Extension;

public static class EntityTypeBuilderQueryFilterExtensions
{
    internal static void AddQueryFilter<T>(this EntityTypeBuilder entityTypeBuilder,
        Expression<Func<T, bool>> expression)
    {
        ParameterExpression parameType = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);
        Expression expressionFilter =
            ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), parameType, expression.Body);

        LambdaExpression currentQueryFilter = entityTypeBuilder.Metadata.GetQueryFilter();
        if (currentQueryFilter is not null)
        {
            Expression currentExpressionFilter =
                ReplacingExpressionVisitor.Replace(currentQueryFilter.Parameters.Single(), parameType,
                    currentQueryFilter.Body);
            expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
        }

        LambdaExpression lambdaExpression = Expression.Lambda(expressionFilter, parameType);
        entityTypeBuilder.HasQueryFilter(lambdaExpression);
    }
}
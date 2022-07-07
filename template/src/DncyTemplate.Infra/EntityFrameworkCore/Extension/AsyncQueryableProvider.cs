using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using DncyTemplate.Domain.Infra;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DncyTemplate.Infra.EntityFrameworkCore.Extension;

public class AsyncQueryableProvider : IAsyncQueryableProvider
{
    public bool CanExecute<T>([NotNull] IQueryable<T> queryable)
    {
        return queryable.Provider is EntityQueryProvider;
    }

    public Task<bool> ContainsAsync<T>([NotNull] IQueryable<T> queryable, T item,
        CancellationToken cancellationToken = default)
    {
        return queryable.ContainsAsync(item, cancellationToken);
    }

    public Task<bool> AnyAsync<T>([NotNull] IQueryable<T> queryable, [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return queryable.AnyAsync(predicate, cancellationToken);
    }

    public Task<bool> AllAsync<T>([NotNull] IQueryable<T> queryable, [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return queryable.AllAsync(predicate, cancellationToken);
    }

    public Task<int> CountAsync<T>([NotNull] IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.CountAsync(cancellationToken);
    }

    public Task<int> CountAsync<T>([NotNull] IQueryable<T> queryable, [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return queryable.CountAsync(predicate, cancellationToken);
    }

    public Task<long> LongCountAsync<T>([NotNull] IQueryable<T> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.LongCountAsync(cancellationToken);
    }

    public Task<long> LongCountAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return queryable.LongCountAsync(predicate, cancellationToken);
    }

    public Task<T> FirstAsync<T>([NotNull] IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.FirstAsync(cancellationToken);
    }

    public Task<T> FirstAsync<T>([NotNull] IQueryable<T> queryable, [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return queryable.FirstAsync(predicate, cancellationToken);
    }

    public Task<T> FirstOrDefaultAsync<T>([NotNull] IQueryable<T> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.FirstOrDefaultAsync(cancellationToken);
    }

    public Task<T> FirstOrDefaultAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<T> LastAsync<T>([NotNull] IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.LastAsync(cancellationToken);
    }

    public Task<T> LastAsync<T>([NotNull] IQueryable<T> queryable, [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return queryable.LastAsync(predicate, cancellationToken);
    }

    public Task<T> LastOrDefaultAsync<T>([NotNull] IQueryable<T> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.LastOrDefaultAsync(cancellationToken);
    }

    public Task<T> LastOrDefaultAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return queryable.LastOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<T> SingleAsync<T>([NotNull] IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SingleAsync(cancellationToken);
    }

    public Task<T> SingleAsync<T>([NotNull] IQueryable<T> queryable, [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return queryable.SingleAsync(predicate, cancellationToken);
    }

    public Task<T> SingleOrDefaultAsync<T>([NotNull] IQueryable<T> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.SingleOrDefaultAsync(cancellationToken);
    }

    public Task<T> SingleOrDefaultAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return queryable.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<T> MinAsync<T>([NotNull] IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.MinAsync(cancellationToken);
    }

    public Task<TResult> MinAsync<T, TResult>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.MinAsync(selector, cancellationToken);
    }

    public Task<T> MaxAsync<T>([NotNull] IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.MaxAsync(cancellationToken);
    }

    public Task<TResult> MaxAsync<T, TResult>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.MaxAsync(selector, cancellationToken);
    }

    public Task<decimal> SumAsync([NotNull] IQueryable<decimal> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<decimal?> SumAsync([NotNull] IQueryable<decimal?> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<decimal> SumAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<decimal?> SumAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<int> SumAsync([NotNull] IQueryable<int> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<int?> SumAsync([NotNull] IQueryable<int?> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<int> SumAsync<T>([NotNull] IQueryable<T> queryable, [NotNull] Expression<Func<T, int>> selector,
        CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<int?> SumAsync<T>([NotNull] IQueryable<T> queryable, [NotNull] Expression<Func<T, int?>> selector,
        CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<long> SumAsync([NotNull] IQueryable<long> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<long?> SumAsync([NotNull] IQueryable<long?> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<long> SumAsync<T>([NotNull] IQueryable<T> queryable, [NotNull] Expression<Func<T, long>> selector,
        CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<long?> SumAsync<T>([NotNull] IQueryable<T> queryable, [NotNull] Expression<Func<T, long?>> selector,
        CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<double> SumAsync([NotNull] IQueryable<double> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<double?> SumAsync([NotNull] IQueryable<double?> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<double> SumAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<double?> SumAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<float> SumAsync([NotNull] IQueryable<float> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<float?> SumAsync([NotNull] IQueryable<float?> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<float> SumAsync<T>([NotNull] IQueryable<T> queryable, [NotNull] Expression<Func<T, float>> selector,
        CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<float?> SumAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<decimal> AverageAsync([NotNull] IQueryable<decimal> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<decimal?> AverageAsync([NotNull] IQueryable<decimal?> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<decimal> AverageAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<decimal?> AverageAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<double> AverageAsync([NotNull] IQueryable<int> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<double?> AverageAsync([NotNull] IQueryable<int?> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<double> AverageAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<double?> AverageAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<double> AverageAsync([NotNull] IQueryable<long> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<double?> AverageAsync([NotNull] IQueryable<long?> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<double> AverageAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<double?> AverageAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<double> AverageAsync([NotNull] IQueryable<double> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<double?> AverageAsync([NotNull] IQueryable<double?> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<double> AverageAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<double?> AverageAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<float> AverageAsync([NotNull] IQueryable<float> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<float?> AverageAsync([NotNull] IQueryable<float?> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<float> AverageAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<float?> AverageAsync<T>([NotNull] IQueryable<T> queryable,
        [NotNull] Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<List<T>> ToListAsync<T>([NotNull] IQueryable<T> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.ToListAsync(cancellationToken);
    }

    public Task<T[]> ToArrayAsync<T>([NotNull] IQueryable<T> queryable,
        CancellationToken cancellationToken = default)
    {
        return queryable.ToArrayAsync(cancellationToken);
    }
}
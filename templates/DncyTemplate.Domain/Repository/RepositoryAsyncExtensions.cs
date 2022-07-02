using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using DncyTemplate.Domain.Infra;

namespace DncyTemplate.Domain.Repository;

public static class RepositoryAsyncExtensions
{
    #region Contains

    public static Task<bool> ContainsAsync<T>([NotNull] this IRepository<T> repository, [NotNull] T item,
        CancellationToken cancellationToken = default) where T : BaseEntity
    {
        return repository.AsyncExecuter.ContainsAsync(repository.Query, item, cancellationToken);
    }

    #endregion

    #region Any/All

    public static Task<bool> AnyAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.AnyAsync(repository.Query, predicate, cancellationToken);
    }

    public static Task<bool> AllAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.AllAsync(repository.Query, predicate, cancellationToken);
    }

    #endregion

    #region Count/LongCount

    public static Task<int> CountAsync<T>([NotNull] this IRepository<T> repository,
        CancellationToken cancellationToken = default) where T : BaseEntity
    {
        return repository.AsyncExecuter.CountAsync(repository.Query, cancellationToken);
    }

    public static Task<int> CountAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.CountAsync(repository.Query, predicate, cancellationToken);
    }

    public static Task<long> LongCountAsync<T>([NotNull] this IRepository<T> repository,
        CancellationToken cancellationToken = default) where T : BaseEntity
    {
        return repository.AsyncExecuter.LongCountAsync(repository.Query, cancellationToken);
    }

    public static Task<long> LongCountAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.LongCountAsync(repository.Query, predicate, cancellationToken);
    }

    #endregion

    #region First/FirstOrDefault

    public static Task<T> FirstAsync<T>([NotNull] this IRepository<T> repository,
        CancellationToken cancellationToken = default) where T : BaseEntity
    {
        return repository.AsyncExecuter.FirstAsync(repository.Query, cancellationToken);
    }

    public static Task<T> FirstAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.FirstAsync(repository.Query, predicate, cancellationToken);
    }

    public static Task<T> FirstOrDefaultAsync<T>([NotNull] this IRepository<T> repository,
        CancellationToken cancellationToken = default) where T : BaseEntity
    {
        return repository.AsyncExecuter.FirstOrDefaultAsync(repository.Query, cancellationToken);
    }

    public static Task<T> FirstOrDefaultAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.FirstOrDefaultAsync(repository.Query, predicate, cancellationToken);
    }

    #endregion

    #region Last/LastOrDefault

    public static Task<T> LastAsync<T>([NotNull] this IRepository<T> repository,
        CancellationToken cancellationToken = default) where T : BaseEntity
    {
        return repository.AsyncExecuter.LastAsync(repository.Query, cancellationToken);
    }

    public static Task<T> LastAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.LastAsync(repository.Query, predicate, cancellationToken);
    }

    public static Task<T> LastOrDefaultAsync<T>([NotNull] this IRepository<T> repository,
        CancellationToken cancellationToken = default) where T : BaseEntity
    {
        return repository.AsyncExecuter.LastOrDefaultAsync(repository.Query, cancellationToken);
    }

    public static Task<T> LastOrDefaultAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.LastOrDefaultAsync(repository.Query, predicate, cancellationToken);
    }

    #endregion

    #region Single/SingleOrDefault

    public static Task<T> SingleAsync<T>([NotNull] this IRepository<T> repository,
        CancellationToken cancellationToken = default) where T : BaseEntity
    {
        return repository.AsyncExecuter.SingleAsync(repository.Query, cancellationToken);
    }

    public static Task<T> SingleAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.SingleAsync(repository.Query, predicate, cancellationToken);
    }

    public static Task<T> SingleOrDefaultAsync<T>([NotNull] this IRepository<T> repository,
        CancellationToken cancellationToken = default) where T : BaseEntity
    {
        return repository.AsyncExecuter.SingleOrDefaultAsync(repository.Query, cancellationToken);
    }

    public static Task<T> SingleOrDefaultAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.SingleOrDefaultAsync(repository.Query, predicate, cancellationToken);
    }

    #endregion

    #region Min

    public static Task<T> MinAsync<T>([NotNull] this IRepository<T> repository,
        CancellationToken cancellationToken = default) where T : BaseEntity
    {
        return repository.AsyncExecuter.MinAsync(repository.Query, cancellationToken);
    }

    public static Task<TResult> MinAsync<T, TResult>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.MinAsync(repository.Query, selector, cancellationToken);
    }

    #endregion

    #region Max

    public static Task<T> MaxAsync<T>([NotNull] this IRepository<T> repository,
        CancellationToken cancellationToken = default) where T : BaseEntity
    {
        return repository.AsyncExecuter.MaxAsync(repository.Query, cancellationToken);
    }

    public static Task<TResult> MaxAsync<T, TResult>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.MaxAsync(repository.Query, selector, cancellationToken);
    }

    #endregion

    #region Sum

    public static Task<decimal> SumAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<decimal?> SumAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<int> SumAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<int?> SumAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<long> SumAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<long?> SumAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<double> SumAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<double?> SumAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<float> SumAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<float?> SumAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.SumAsync(repository.Query, selector, cancellationToken);
    }

    #endregion

    #region Average

    public static Task<decimal> AverageAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<decimal?> AverageAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<double> AverageAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<double?> AverageAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<double> AverageAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<double?> AverageAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<double> AverageAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<double?> AverageAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
    }

    public static Task<float?> AverageAsync<T>([NotNull] this IRepository<T> repository,
        [NotNull] Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return repository.AsyncExecuter.AverageAsync(repository.Query, selector, cancellationToken);
    }

    #endregion

    #region ToList/Array

    public static Task<List<T>> ToListAsync<T>([NotNull] this IRepository<T> repository,
        CancellationToken cancellationToken = default) where T : BaseEntity
    {
        return repository.AsyncExecuter.ToListAsync(repository.Query, cancellationToken);
    }

    public static Task<T[]> ToArrayAsync<T>([NotNull] this IRepository<T> repository,
        CancellationToken cancellationToken = default) where T : BaseEntity
    {
        return repository.AsyncExecuter.ToArrayAsync(repository.Query, cancellationToken);
    }

    #endregion
}
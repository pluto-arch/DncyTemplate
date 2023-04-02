using DncyTemplate.Domain.Infra;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace DncyTemplate.Infra.EntityFrameworkCore.Repository
{
    public static class EfCoreRepositoryAsyncExtensions
    {
        #region Contains

        public static Task<bool> ContainsAsync<T>([NotNull] this IEfRepository<T> repository, [NotNull] T item,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.ContainsAsync(repository.DbSet, item, cancellationToken);
        }

        #endregion

        #region Any/All

        public static Task<bool> AnyAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.AnyAsync(repository.DbSet, predicate, cancellationToken);
        }

        public static Task<bool> AllAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.AllAsync(repository.DbSet, predicate, cancellationToken);
        }

        #endregion

        #region Count/LongCount

        public static Task<int> CountAsync<T>([NotNull] this IEfRepository<T> repository,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.CountAsync(repository.DbSet, cancellationToken);
        }

        public static Task<int> CountAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.CountAsync(repository.DbSet, predicate, cancellationToken);
        }

        public static Task<long> LongCountAsync<T>([NotNull] this IEfRepository<T> repository,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.LongCountAsync(repository.DbSet, cancellationToken);
        }

        public static Task<long> LongCountAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.LongCountAsync(repository.DbSet, predicate, cancellationToken);
        }

        #endregion

        #region First/FirstOrDefault

        public static Task<T> FirstAsync<T>([NotNull] this IEfRepository<T> repository,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.FirstAsync(repository.DbSet, cancellationToken);
        }

        public static Task<T> FirstAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.FirstAsync(repository.DbSet, predicate, cancellationToken);
        }

        public static Task<T> FirstOrDefaultAsync<T>([NotNull] this IEfRepository<T> repository,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.FirstOrDefaultAsync(repository.DbSet, cancellationToken);
        }

        public static Task<T> FirstOrDefaultAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.FirstOrDefaultAsync(repository.DbSet, predicate, cancellationToken);
        }

        #endregion

        #region Last/LastOrDefault

        public static Task<T> LastAsync<T>([NotNull] this IEfRepository<T> repository,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.LastAsync(repository.DbSet, cancellationToken);
        }

        public static Task<T> LastAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.LastAsync(repository.DbSet, predicate, cancellationToken);
        }

        public static Task<T> LastOrDefaultAsync<T>([NotNull] this IEfRepository<T> repository,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.LastOrDefaultAsync(repository.DbSet, cancellationToken);
        }

        public static Task<T> LastOrDefaultAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.LastOrDefaultAsync(repository.DbSet, predicate, cancellationToken);
        }

        #endregion

        #region Single/SingleOrDefault

        public static Task<T> SingleAsync<T>([NotNull] this IEfRepository<T> repository,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SingleAsync(repository.DbSet, cancellationToken);
        }

        public static Task<T> SingleAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SingleAsync(repository.DbSet, predicate, cancellationToken);
        }

        public static Task<T> SingleOrDefaultAsync<T>([NotNull] this IEfRepository<T> repository,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.SingleOrDefaultAsync(repository.DbSet, cancellationToken);
        }

        public static Task<T> SingleOrDefaultAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SingleOrDefaultAsync(repository.DbSet, predicate, cancellationToken);
        }

        #endregion

        #region Min

        public static Task<T> MinAsync<T>([NotNull] this IEfRepository<T> repository,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.MinAsync(repository.DbSet, cancellationToken);
        }

        public static Task<TResult> MinAsync<T, TResult>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.MinAsync(repository.DbSet, selector, cancellationToken);
        }

        #endregion

        #region Max

        public static Task<T> MaxAsync<T>([NotNull] this IEfRepository<T> repository,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.MaxAsync(repository.DbSet, cancellationToken);
        }

        public static Task<TResult> MaxAsync<T, TResult>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.MaxAsync(repository.DbSet, selector, cancellationToken);
        }

        #endregion

        #region Sum

        public static Task<decimal> SumAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<decimal?> SumAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<int> SumAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<int?> SumAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<long> SumAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<long?> SumAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<double> SumAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<double?> SumAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<float> SumAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<float?> SumAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.SumAsync(repository.DbSet, selector, cancellationToken);
        }

        #endregion

        #region Average

        public static Task<decimal> AverageAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<decimal?> AverageAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<double> AverageAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<double?> AverageAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<double> AverageAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<double?> AverageAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<double> AverageAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<double?> AverageAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.DbSet, selector, cancellationToken);
        }

        public static Task<float?> AverageAsync<T>([NotNull] this IEfRepository<T> repository,
            [NotNull] Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
            where T : BaseEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository.DbSet, selector, cancellationToken);
        }

        #endregion

        #region ToList/Array

        public static Task<List<T>> ToListAsync<T>([NotNull] this IEfRepository<T> repository,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.ToListAsync(repository.DbSet, cancellationToken);
        }

        public static Task<T[]> ToArrayAsync<T>([NotNull] this IEfRepository<T> repository,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            return repository.AsyncExecuter.ToArrayAsync(repository.DbSet, cancellationToken);
        }

        #endregion
    }
}

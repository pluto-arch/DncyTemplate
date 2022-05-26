using DncyTemplate.Domain.Collections;
using Microsoft.EntityFrameworkCore;

namespace DncyTemplate.Infra.EntityFrameworkCore.Extension;

public static class QueryablePageListExtensions
{
    /// <summary>
    /// 转分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex,
        int pageSize, CancellationToken cancellationToken = default)
    {
        if (pageIndex < 1)
        {
            throw new ArgumentException("页码不能小于1");
        }

        int count = await source.CountAsync(cancellationToken);
        List<T> items = await source.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize).ToListAsync(cancellationToken);

        PagedList<T> pagedList = new()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = count,
            Items = items
        };

        return pagedList;
    }
}
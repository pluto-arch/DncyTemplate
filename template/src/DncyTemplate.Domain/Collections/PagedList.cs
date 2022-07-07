namespace DncyTemplate.Domain.Collections;

public class PagedList<T> : IPagedList<T>
{
    /// <summary>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    public PagedList(IEnumerable<T> source, int pageIndex, int pageSize)
    {
        if (source == null)
        {
            throw new NullReferenceException("数据源不能为空");
        }

        if (pageIndex < 1)
        {
            throw new ArgumentException("页码不能小于1");
        }

        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = source.Count();
        if (TotalCount > pageSize)
        {
            Items = source.Skip(( PageIndex - 1 ) * PageSize).Take(PageSize).ToList();
        }

        Items = source.ToList();
    }

    /// <summary>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="total"></param>
    public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int total)
    {
        if (source == null)
        {
            throw new NullReferenceException("数据源不能为空");
        }

        if (pageIndex < 1)
        {
            throw new ArgumentException("页码不能小于1");
        }

        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = total;
        if (total > pageSize)
        {
            Items = source.Skip(( PageIndex - 1 ) * PageSize).Take(PageSize).ToList();
        }

        Items = source.ToList();
    }


    public PagedList()
    {
        Items = Array.Empty<T>();
    }

    /// <inheritdoc />
    public int PageIndex { get; set; }

    /// <inheritdoc />
    public int PageSize { get; set; }

    /// <inheritdoc />
    public int TotalCount { get; set; }

    /// <inheritdoc />
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <inheritdoc />
    public bool HasPreviousPage => PageIndex - 1 > 0;

    /// <inheritdoc />
    public bool HasNextPage => PageIndex < TotalPages;

    /// <inheritdoc />
    public IList<T> Items { get; set; }
}

public class PagedList<TSource, TResult> : IPagedList<TResult>
{
    public PagedList(
        IEnumerable<TSource> source,
        Func<IEnumerable<TSource>, IEnumerable<TResult>> converter,
        int pageIndex,
        int pageSize)
    {
        if (source == null)
        {
            throw new NullReferenceException("数据源不能为空");
        }

        if (pageIndex < 1)
        {
            throw new ArgumentException("页码不能小于1");
        }

        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = source.Count();
        if (TotalCount > pageSize)
        {
            TSource[] items = source.Skip(( PageIndex - 1 ) * PageSize).Take(PageSize).ToArray();
            Items = new List<TResult>(converter(items));
        }
        else
        {
            Items = new List<TResult>(converter(source));
        }
    }


    public PagedList(
        IPagedList<TSource> source,
        Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
    {
        PageIndex = source.PageIndex;
        PageSize = source.PageSize;
        TotalCount = source.TotalCount;
        Items = new List<TResult>(converter(source.Items));
    }

    public int PageIndex { get; }

    public int PageSize { get; }

    public int TotalCount { get; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPreviousPage => PageIndex - 1 > 0;

    public bool HasNextPage => PageIndex < TotalPages;

    public IList<TResult> Items { get; }
}

public static class PagedList
{
    /// <summary>
    ///     return empty IPageList
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IPagedList<T> Empty<T>()
    {
        return new PagedList<T>();
    }

    /// <summary>
    ///     converter to IPageList
    /// </summary>
    /// <param name="source"></param>
    /// <param name="converter"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static IPagedList<TResult> From<TResult, TSource>(
        IPagedList<TSource> source,
        Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
    {
        return new PagedList<TSource, TResult>(source, converter);
    }
}
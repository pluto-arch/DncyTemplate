namespace Dncy.Specifications.Builder;

public class CacheSpecificationBuilder<T> : ICacheSpecificationBuilder<T> where T : class
{
    public CacheSpecificationBuilder(Specification<T> specification)
    {
        Specification = specification;
    }

    public Specification<T> Specification { get; }
}
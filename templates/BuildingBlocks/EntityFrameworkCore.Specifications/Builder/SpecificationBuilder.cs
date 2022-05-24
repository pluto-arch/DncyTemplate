namespace Dncy.Specifications.Builder;

public class SpecificationBuilder<T, TResult> : SpecificationBuilder<T>, ISpecificationBuilder<T, TResult>
{
    public SpecificationBuilder(Specification<T, TResult> specification)
        : base(specification)
    {
        Specification = specification;
    }

    public new Specification<T, TResult> Specification { get; }
}

public class SpecificationBuilder<T> : ISpecificationBuilder<T>
{
    public SpecificationBuilder(Specification<T> specification)
    {
        Specification = specification;
    }

    public Specification<T> Specification { get; }
}
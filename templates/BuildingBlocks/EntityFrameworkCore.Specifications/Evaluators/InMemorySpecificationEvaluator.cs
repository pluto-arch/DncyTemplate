using Dncy.Specifications.Exceptions;

namespace Dncy.Specifications.Evaluators;

public class InMemorySpecificationEvaluator : IInMemorySpecificationEvaluator
{
    private readonly List<IInMemoryEvaluator> evaluators = new();

    public InMemorySpecificationEvaluator()
    {
        evaluators.AddRange(new IInMemoryEvaluator[]
        {
            WhereEvaluator.Instance, OrderEvaluator.Instance, PaginationEvaluator.Instance
        });
    }

    public InMemorySpecificationEvaluator(IEnumerable<IInMemoryEvaluator> evaluators)
    {
        this.evaluators.AddRange(evaluators);
    }

    public static InMemorySpecificationEvaluator Default { get; } = new();

    public virtual IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source,
        ISpecification<T, TResult> specification)
    {
        _ = specification.Selector ?? throw new SelectorNotFoundException();

        IEnumerable<T> baseQuery = Evaluate(source, (ISpecification<T>)specification);

        IEnumerable<TResult> resultQuery = baseQuery.Select(specification.Selector.Compile());

        return specification.PostProcessingAction == null
            ? resultQuery
            : specification.PostProcessingAction(resultQuery);
    }

    public virtual IEnumerable<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification)
    {
        if (specification.SearchCriterias.Any())
        {
            throw new NotSupportedException(
                "The specification contains Search expressions and can't be evaluated with in-memory evaluator.");
        }

        foreach (IInMemoryEvaluator evaluator in evaluators)
        {
            source = evaluator.Evaluate(source, specification);
        }

        return specification.PostProcessingAction == null ? source : specification.PostProcessingAction(source);
    }


}
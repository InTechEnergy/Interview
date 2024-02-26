namespace ExampleApp.Api.Utils;

internal class SpecificationEvaluator : ISpecificationEvaluator
{
    public IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification) where T : notnull
    {
        inputQuery = inputQuery.Where(specification.Criteria);

        return inputQuery;
    }
}

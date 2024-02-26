namespace ExampleApp.Api.Utils;

internal interface ISpecificationEvaluator
{
    IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification);
}

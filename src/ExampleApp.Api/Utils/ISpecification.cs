using System.Linq.Expressions;

namespace ExampleApp.Api.Utils;

internal interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
}

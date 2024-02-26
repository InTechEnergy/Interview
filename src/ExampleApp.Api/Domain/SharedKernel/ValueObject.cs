namespace ExampleApp.Api.Domain.SharedKernel;

internal class ValueObject<T> where T: notnull
{
    public T Id { get; protected init; } = default(T)!;
}

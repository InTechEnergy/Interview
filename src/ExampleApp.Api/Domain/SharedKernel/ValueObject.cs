using ExampleApp.Api.Domain.SharedKernel.Contracts;

namespace ExampleApp.Api.Domain.SharedKernel;

internal class ValueObject<T> : IAuditable
    where T: notnull
{
    public T Id { get; protected init; } = default(T)!;
    public DateTime CreatedOn { get; set; }
    public DateTime ChangedOn { get; set; }
    public bool IsDeleted { get; set; }
}

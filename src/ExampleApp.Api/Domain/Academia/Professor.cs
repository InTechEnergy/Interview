using ExampleApp.Api.Domain.SharedKernel;

namespace ExampleApp.Api.Domain.Academia;

internal class Professor : ValueObject<Guid>
{
    public string FullName { get; init; } = "TBD";
    public string? Extension { get; init; }
}

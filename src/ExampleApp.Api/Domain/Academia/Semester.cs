using ExampleApp.Api.Domain.SharedKernel;

namespace ExampleApp.Api.Domain.Academia;

internal class Semester : ValueObject<Guid>
{
    public string Description { get; init; } = "Description";
    public DateOnly Start { get; init; } = default;
    public DateOnly End { get; init; } = default;
}

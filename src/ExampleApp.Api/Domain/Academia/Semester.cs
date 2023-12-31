namespace ExampleApp.Api.Domain.Academia;

internal class Semester : ValueObject<string>
{
    public string Description { get; init; } = "Description";
    public DateOnly Start { get; init; } = default;
    public DateOnly End { get; init; } = default;
}

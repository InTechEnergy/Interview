namespace ExampleApp.Api.Domain.Academia;

internal class Semester : ValueObject<string>
{
    public Semester(string id)
    {
        Id = id;
    }

    public string Description { get; init; } = "Description";
    public DateOnly Start { get; init; } = default;
    public DateOnly End { get; init; } = default;
}

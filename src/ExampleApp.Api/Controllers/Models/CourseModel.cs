namespace ExampleApp.Api.Controllers.Models;

public record CourseModel : KeyNameModel
{
    public CourseModel(string key, string name) : base(key, name)
    {
    }

    public required ProfessorModel Professor { get; set; }
}

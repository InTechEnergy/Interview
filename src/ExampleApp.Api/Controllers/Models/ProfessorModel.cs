namespace ExampleApp.Api.Controllers.Models;

public record ProfessorModel : KeyNameModel
{
    public ProfessorModel(string key, string name) : base(key, name)
    {
    }
}

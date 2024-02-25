namespace ExampleApp.Api.Controllers.Models;

public class ModelStateError
{
    public required string FieldName { get; set; }
    public IEnumerable<string>? ErrorMessage { get; set; }
}

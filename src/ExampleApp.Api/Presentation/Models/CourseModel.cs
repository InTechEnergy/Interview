namespace ExampleApp.Api.Controllers.Models;

public record CourseModel(Guid Key, string Name, KeyNameModel Professor) : KeyNameModel(Key, Name);

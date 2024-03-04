namespace ExampleApp.Api.Controllers.Models;

public record CourseModel(string Id, string Name, KeyNameModel Professor) : KeyNameModel(Id, Name);

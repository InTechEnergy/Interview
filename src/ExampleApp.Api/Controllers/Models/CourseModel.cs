namespace ExampleApp.Api.Controllers.Models;

public record CourseModel(string Id, string Description, KeyNameModel Semester, KeyNameModel Professor);

namespace ExampleApp.Api.Controllers.Models;

public record CourseModel(Guid Id, string Description, KeyNameModel Semester, KeyNameModel Professor);

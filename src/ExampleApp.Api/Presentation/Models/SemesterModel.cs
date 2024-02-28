namespace ExampleApp.Api.Controllers.Models;

public record SemesterModel(
    Guid Key,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    IReadOnlyList<CourseModel> Courses) : KeyNameModel(Key, Name);

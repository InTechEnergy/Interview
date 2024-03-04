namespace ExampleApp.Api.Controllers.Models;

public record SemesterModel(string Key, string Name, DateOnly Start, DateOnly End, IReadOnlyList<CourseModel> Courses) : KeyNameModel(Key, Name);

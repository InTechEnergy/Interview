namespace ExampleApp.Api.Domain.Academia.QueryHandlers.Students;

public sealed record StudentCourseView(StudentView Student, IReadOnlyList<Guid> CourseIds);

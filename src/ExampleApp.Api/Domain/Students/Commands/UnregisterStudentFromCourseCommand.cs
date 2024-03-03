using MediatR;

namespace ExampleApp.Api.Domain.Students.Commands;

internal record UnregisterStudentFromCourseCommand(int StudentId, string CourseId) : IRequest<Unit>;

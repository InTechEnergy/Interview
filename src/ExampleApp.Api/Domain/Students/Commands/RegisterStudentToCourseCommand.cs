using MediatR;

namespace ExampleApp.Api.Domain.Students.Commands;

internal record RegisterStudentToCourseCommand(int StudentId, string CourseId) : IRequest<Unit>;

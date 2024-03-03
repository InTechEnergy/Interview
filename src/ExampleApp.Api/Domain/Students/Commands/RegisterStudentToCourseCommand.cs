using MediatR;

namespace ExampleApp.Api.Domain.Students.Commands;

internal record RegisterStudentToCourseCommand(int studentId, string courseId) : IRequest<Unit>;

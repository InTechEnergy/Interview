using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record IsStudentEnrolledInCourseQuery(int studentId, string courseId) : IRequest<bool>;

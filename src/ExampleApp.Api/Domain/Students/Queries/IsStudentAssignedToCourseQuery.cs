using MediatR;

namespace ExampleApp.Api.Domain.Students.Queries;

internal record IsStudentAssignedToCourseQuery(int studentId, string courseId) : IRequest<bool>;

using MediatR;

namespace ExampleApp.Api.Domain.Academia.Commands;

internal record UpdateCourseProfessor(Guid CourseId, Guid NewProfessorId) : IRequest<Unit>;

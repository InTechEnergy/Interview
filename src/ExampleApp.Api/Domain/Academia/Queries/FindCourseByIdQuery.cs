using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record FindCourseByIdQuery(Guid Id) : IRequest<Course?>;

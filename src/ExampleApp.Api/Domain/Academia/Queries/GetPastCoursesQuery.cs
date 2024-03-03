using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record GetPastCoursesQuery() : IRequest<ICollection<Course>>;

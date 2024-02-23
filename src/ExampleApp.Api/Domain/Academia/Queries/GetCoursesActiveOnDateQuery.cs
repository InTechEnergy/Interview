using ExampleApp.Api.Domain.Students;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record GetStudentsActiveOnDateQuery(DateOnly ActiveOn) : IRequest<ICollection<Student>>;

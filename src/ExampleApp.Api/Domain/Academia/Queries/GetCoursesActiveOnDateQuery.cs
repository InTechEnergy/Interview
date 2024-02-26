using ExampleApp.Api.Domain.SharedKernel.Entities;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record GetCoursesActiveOnDateQuery(DateOnly ActiveOn) : IRequest<ICollection<Course>>;

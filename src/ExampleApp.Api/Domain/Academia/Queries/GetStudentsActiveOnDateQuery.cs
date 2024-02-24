using ExampleApp.Api.Domain.Academia.Models;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record GetStudentsActiveOnDateQuery(DateOnly ActiveOn) : IRequest<ICollection<StudentCourseCount>>;

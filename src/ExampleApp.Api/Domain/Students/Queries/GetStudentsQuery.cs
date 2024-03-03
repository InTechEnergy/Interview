using ExampleApp.Api.Domain.Academia.Models;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record GetStudentsQuery(DateOnly ActiveOn) : IRequest<ICollection<StudentCourseCountModel>>;

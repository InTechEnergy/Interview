using ExampleApp.Api.Domain.Academia.QueryHandlers.Students;
using ExampleApp.Api.Utils.Models;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries.Students;

public sealed record GetStudentCoursesByCurrentSemesterQuery() : IRequest<Result<IReadOnlyList<StudentCourseView>>>;

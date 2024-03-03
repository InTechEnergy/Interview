using ExampleApp.Api.Controllers.Models;
using MediatR;

namespace ExampleApp.Api.Domain.Students.Queries;

internal record FindStudentQuery(string? fullName, string? badgeNumber) : IRequest<Student?>;

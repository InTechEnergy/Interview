using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Utils.Models;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.Commands;

public record UnsubscribeStudentToCourseCommand(StudentToCourseModel StudentCourse) : IRequest<Result<Unit>>;

using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Utils.Models;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.Commands;

public record SubscribeStudentToCourseCommand(SubscribeStudentToCourseModel StudentCourse) : IRequest<Result<Unit>>;

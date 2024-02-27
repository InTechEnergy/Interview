using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia.Queries.Students;
using ExampleApp.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp.Api.Controllers;

public class CoursesEndpoint
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/courses/{courseId}", static ([FromBody] SubscribeStudentToCourseModel model, [FromServices] IMediator mediator)
            => mediator.SendCommand(new GetStudentCoursesByCurrentSemesterQuery()))
            .Produces<Unit>()
            .WithDisplayName("Associate Student to Course")
            .WithTags("Courses");;
    }
}

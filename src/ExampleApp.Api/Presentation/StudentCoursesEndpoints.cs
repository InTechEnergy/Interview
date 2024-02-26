using ExampleApp.Api.Domain.Academia.Queries.Students;
using ExampleApp.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp.Api.Controllers;

public static class StudentCoursesEndpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/student-courses", static ([FromServices] IMediator mediator)
                => mediator.SendCommand(new GetStudentCoursesByCurrentSemesterQuery()));
    }
}

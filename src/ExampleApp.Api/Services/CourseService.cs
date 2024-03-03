using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Services.Interfaces;
using MediatR;

namespace ExampleApp.Api.Services;

public class CourseService : ICourseService
{
    private readonly IMediator _mediator;

    public CourseService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> IsCourseCurrent(string courseId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var activeCourses = await _mediator.Send(new GetCoursesActiveOnDateQuery(today));

        return activeCourses.Any(c => c.Id == courseId);
    }
}

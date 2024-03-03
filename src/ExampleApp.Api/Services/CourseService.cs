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

    public async Task<bool> IsCourseCurrentAsync(string courseId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var activeCourses = await _mediator.Send(new GetCoursesActiveOnDateQuery(today));

        return activeCourses.Any(c => c.Id == courseId);
    }

    public async Task<bool> IsCoursePastAsync(string courseId)
    {
        var activeCourses = await _mediator.Send(new GetPastCoursesQuery());

        return activeCourses.Any(c => c.Id == courseId);
    }
}

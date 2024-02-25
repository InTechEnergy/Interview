using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Interfaces;
using MediatR;

namespace ExampleApp.Api.Services;

public class CoursesService : ICoursesService
{
    private readonly IMediator _mediator;

    public CoursesService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> IsCourseActive(string courseId)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        ICollection<Course> activeCourses = await _mediator.Send(new GetCoursesActiveOnDateQuery(today));

        return activeCourses.Any(c => c.Id == courseId);
    }
}

using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Academia;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Domain.Academia.Models;

namespace ExampleApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CoursesController> _logger;

    public StudentsController(IMediator mediator, ILogger<CoursesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet(Name = "GetStudents")]
    public async Task<IEnumerable<StudentModel>> GetCurrent()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        ICollection<StudentCourseCountModel> students = await _mediator.Send(new GetStudentsActiveOnDateQuery(today));
        _logger.LogInformation("Retrieved {Count} current students", students.Count);

        return students
            .Select(student =>
                new StudentModel(
                    student.Student.Id,
                    student.Student.FullName,
                    student.Student.Badge,
                    student.CourseCount)
                )
            .ToList()
            .OrderByDescending(x => x.courseCount)
            .ThenBy(x => x.fullName);
    }
}

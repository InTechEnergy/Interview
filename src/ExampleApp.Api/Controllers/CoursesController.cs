using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.Academia.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(IMediator mediator, ILogger<CoursesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet(Name = "GetCurrentCourses")]
    public async Task<IActionResult> GetCurrent()
    {
        DateOnly today = new(2023, 9, 1);
        ICollection<Course> courses = await _mediator.Send(new GetCoursesActiveOnDateQuery(today));

        if(!courses.Any())
        {
            return StatusCode(204, new { });
        }

        _logger.LogInformation("Retrieved {Count} current courses", courses.Count);

        Course firstCourse = courses.First();
        ResponseCoursesSemesterModel response = new()
        {
            Semester = new SemesterCoursesResponseModel(firstCourse.Semester.Id, firstCourse.Semester.Description)
            {
                StartDate = firstCourse.Semester.Start,
                EndDate = firstCourse.Semester.End,
                Courses = courses.Select(CreateCourseModel).ToList()
            }
        };

        return Ok(response);
    }

    [HttpPatch(Name = "UpdatesProfessor")]
    public async Task<ActionResult> UpdateProfessor([FromBody] ProfessorUpdateModel model)
    {
        var existingCourse = await _mediator.Send(new FindCourseByIdQuery(model.CourseId));
        if (existingCourse is null)
        {
            return NotFound($"Invalid course {model.CourseId}");
        }

        var professor = await _mediator.Send(new FindProfessorByNamedQuery(model.NewProfessorName));
        if (professor is null)
        {
            return NotFound($"Cannot file a professor named {model.NewProfessorName}");
        }

        _ = await _mediator.Send(new UpdateCourseProfessor(existingCourse.Id, professor.Id));
        return Accepted();
    }

    private static CourseModel CreateCourseModel(Course course)
    {
        return new CourseModel(course.Id, course.Description)
        {
            Professor = new ProfessorModel(course.Lecturer.Id.ToString(), course.Lecturer.FullName),
        };
    }
}

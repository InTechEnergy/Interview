using ExampleApp.Api.Common;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Academia.Models;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp.Api.Controllers;

/// <summary>
/// The controller for the students resource.
/// </summary>
[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IMediator mediator,
        ILogger<StudentsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets the students currently enrolled in the academy.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Enrrolls a student in a course.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("student-enroll")]
    public async Task<IActionResult> EnrollStudentInCourse([FromBody] StudentEnrollmentCourseRequestModel request)
    {
        try
        {
            if (request is null)
            {
                return BadRequest(Constants.BODY_REQUEST_IS_REQUIRED);
            }

            if (!ModelState.IsValid)
            {
                List<ModelStateError> errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .Select(x => new ModelStateError()
                    {
                        FieldName = x.Key,
                        ErrorMessage = x.Value?.Errors.Select(e => e.ErrorMessage)
                    })
                    .ToList();

                return BadRequest(errors);
            }

            Course? course = await _mediator.Send(new FindCourseByIdQuery(request.CourseId));

            if (course is null)
            {
                return NotFound($"Course {request.CourseId} not found.");
            }
            else
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
                ICollection<Course> activeCourses = await _mediator.Send(new GetCoursesActiveOnDateQuery(today));

                if(!activeCourses.Contains(course))
                {
                    return BadRequest($"Course {course.Id} is not active today.");
                }
            }

            Student? student = await _mediator.Send(new FindStudentQuery(request));

            if (student is null)
            {
                string message = $"{request.FullName} {request.BadgeNumber}".Trim();
                return NotFound($"Student {message} not found.");
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{Constants.ERROR_OCCURRED} {ex.Message}");
        }
    }

}

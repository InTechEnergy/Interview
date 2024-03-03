using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia.Models;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students.Commands;
using ExampleApp.Api.Domain.Students.Queries;
using ExampleApp.Api.Services.Interfaces;
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
    private readonly ICourseService _courseService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(
        IMediator mediator,
        ICourseService courseService,
        ILogger<StudentsController> logger)
    {
        _mediator = mediator;
        _courseService = courseService;
        _logger = logger;
    }

    [HttpGet(Name = "GetStudents")]
    public async Task<IEnumerable<StudentModel>> GetCurrent()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        ICollection<StudentCourseCountModel> students = await _mediator.Send(new GetStudentsQuery(today));
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
            .OrderBy(x => x.courseCount);
    }

    [Route("register-to-course")]
    [HttpPost(Name = "RegisterStudentToCourse")]
    public async Task<IActionResult> RegisterStudentToCourse([FromBody] StudentToCourseEnrollmentModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.Values);
        }

        if (string.IsNullOrEmpty(request.FullName) && string.IsNullOrEmpty(request.BadgeNumber))
        {
            return BadRequest("FullName or BadgeNumber is required.");
        }

        var isCourseCurrent = await _courseService.IsCourseCurrentAsync(request.CourseId!);
        if (!isCourseCurrent)
        {
            return BadRequest($"Course {request.CourseId} is not available.");
        }

        var course = await _mediator.Send(new FindCourseByIdQuery(request.CourseId));
        if (course is null)
        {
            return NotFound($"Course {request.CourseId} is not found.");
        }

        var student = await _mediator.Send(new FindStudentQuery(request.FullName, request.BadgeNumber));
        if (student is null)
        {
            var studentId = string.IsNullOrWhiteSpace(request.FullName) ? request.FullName : request.BadgeNumber;
            return NotFound($"Student '{studentId}' is not found.");
        }

        bool isStudentEnrolled = await _mediator.Send(new IsStudentAssignedToCourseQuery(student.Id, request.CourseId));
        if (isStudentEnrolled)
        {
            return BadRequest($"Student {student.Id} is already enrolled in course {request.CourseId}.");
        }

        var registerResult = await _mediator.Send(new RegisterStudentToCourseCommand(student.Id, request.CourseId));
        return Created(nameof(RegisterStudentToCourse), registerResult);
    }

    [Route("unregister-from-course")]
    [HttpPost(Name = "UnregisterStudentToCourse")]
    public async Task<IActionResult> UnregisterStudentToCourse([FromBody] StudentToCourseEnrollmentModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.Values);
        }

        if (string.IsNullOrEmpty(request.FullName) && string.IsNullOrEmpty(request.BadgeNumber))
        {
            return BadRequest("FullName or BadgeNumber is required.");
        }

        var isCoursePast = await _courseService.IsCoursePastAsync(request.CourseId!);
        if (!isCoursePast)
        {
            return BadRequest($"Course {request.CourseId} should be available.");
        }

        var course = await _mediator.Send(new FindCourseByIdQuery(request.CourseId));
        if (course is null)
        {
            return NotFound($"Course {request.CourseId} is not found.");
        }

        var student = await _mediator.Send(new FindStudentQuery(request.FullName, request.BadgeNumber));
        if (student is null)
        {
            var studentId = string.IsNullOrWhiteSpace(request.FullName) ? request.FullName : request.BadgeNumber;
            return NotFound($"Student '{studentId}' is not found.");
        }

        bool isStudentEnrolled = await _mediator.Send(new IsStudentAssignedToCourseQuery(student.Id, request.CourseId));
        if (isStudentEnrolled)
        {
            return BadRequest($"Student {student.Id} is already enrolled in course {request.CourseId}.");
        }

        var unregisterResult = await _mediator.Send(new UnregisterStudentFromCourseCommand(student.Id, request.CourseId));
        return Created(nameof(RegisterStudentToCourse), unregisterResult);
    }
}

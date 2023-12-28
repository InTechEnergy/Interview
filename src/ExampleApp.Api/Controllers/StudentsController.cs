using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IMediator mediator, ILogger<StudentsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet(Name = "GetStudents")]
    public async Task<IEnumerable<StudentModel>> GetAll()
    {
        DateOnly today = new(2023, 12, 1);
        ICollection<StudentModel> students = await _mediator.Send(new GetAllStudentsQuery(today));
        _logger.LogInformation("Retrieved {Count} students", students.Count);

        return students;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterInCourse([FromBody] RegisterStudentInCourse command)
    {
        bool registrationResult = await _mediator.Send(command);

        if (registrationResult)
        {
            _logger.LogInformation("Student registered in course {CourseId}", command.CourseId);
            return Ok("Registration successful");
        }

        _logger.LogWarning("Failed to register student in course {CourseId}", command.CourseId);
        return BadRequest("Registration failed");
    }

    [HttpPost("Unregister")]
    public async Task<IActionResult> UnregisterFromCourse([FromBody] UnregisterStudentInCourse command)
    {
        bool unregistrationResult = await _mediator.Send(command);

        if (unregistrationResult)
        {
            _logger.LogInformation("Student un-registered from course {CourseId}", command.CourseId);
            return Ok("Un-registration successful");
        }

        _logger.LogWarning("Un-registration failed");
        return BadRequest("Un-registration failed");
    }


}

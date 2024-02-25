using Azure;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Academia.Models;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

    public StudentsController(IMediator mediator, ILogger<StudentsController> logger)
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
    /// Gets the students currently enrolled in the academy.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("student-enroll")]
    public async Task<IActionResult> EnrollStudentInCourse([FromBody] StudentEnrollmentCourseRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Course? course = await _mediator.Send(new FindCourseByIdQuery(request.CourseId));
            if (course is null)
            {
                return NotFound($"Course {request.CourseId} not found.");
            }

            Student? student = await _mediator.Send(new FindStudentQuery(request));
            if (student is null)
            {
                return NotFound($"Student {student} not found.");
            }


            //// Assume that _enrollmentService is a service that handles the enrollment process
            //var enrollment = await _enrollmentService.EnrollStudentInCourse(studentId, courseId);

            //if (enrollment == null)
            //{
            //    return NotFound();
            //}

            //return CreatedAtAction(nameof(GetEnrollment), new { studentId = studentId, courseId = courseId }, enrollment);
            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception here
            return BadRequest(ex.Message);
        }
    }

}

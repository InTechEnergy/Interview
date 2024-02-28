﻿using ExampleApp.Api.Common;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.Academia.Models;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Interfaces;
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
    private readonly IBulkService _bulkService;
    private readonly ICoursesService _coursesService;
    private readonly List<IFileProcessorService> _fileProcessors;
    private readonly IValidationsService _validationsService;

    public StudentsController(IMediator mediator,
        ILogger<StudentsController> logger,
        IBulkService bulkService,
        ICoursesService coursesService,
        IEnumerable<IFileProcessorService> fileProcessors,
        IValidationsService validationsService)
    {
        _mediator = mediator;
        _logger = logger;
        _bulkService = bulkService;
        _coursesService = coursesService;
        _fileProcessors = fileProcessors.ToList();
        _validationsService = validationsService;
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
        List<ModelStateError> errors = _validationsService.CheckErrorsAsync(request, ModelState);

        if (errors.Any())
        {
            return BadRequest(errors);
        }

        bool isCourseActive = await _coursesService.IsCourseActive(request.CourseId);

        if (!isCourseActive)
        {
            return BadRequest(new ModelStateError() { FieldName = "CourseId", ErrorMessage = new[] { $"Course {request.CourseId} is not currently active." } });
        }

        Student? student = await _mediator.Send(new FindStudentQuery(request));

        if (student is null)
        {
            string message = $"{request.FullName} {request.BadgeNumber}".Trim();
            return BadRequest(new ModelStateError() { FieldName = "Student", ErrorMessage = new[] { $"Student {message} not found." } });
        }

        bool isStudentEnrolled = await _mediator.Send(new IsStudentEnrolledInCourseQuery(student.Id, request.CourseId));

        if (isStudentEnrolled)
        {
            return BadRequest(new ModelStateError() { FieldName = "Student", ErrorMessage = new[] { $"Student {student.Id} is already enrolled in course {request.CourseId}." } });
        }

        Unit studentCourse = await _mediator.Send(new CreateStudentCourse(student.Id, request.CourseId));

        return CreatedAtAction("EnrollStudentInCourse", studentCourse);
    }

    /// <summary>
    /// Student enroll bulk.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("student-enroll-bulk")]
    public async Task<IActionResult> StudentEnrrollBulk(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        string contentType = file.ContentType;
        string extension = Path.GetExtension(file.FileName);

        IFileProcessorService? processor = _fileProcessors.FirstOrDefault(p => p.CanProcess(contentType, extension));

        if (processor == null)
        {
            return BadRequest(Constants.INVALID_FILE_TYPE);
        }

        try
        {
            List<StudentEnrollmentCourseBulkRequestModel> records = processor.Process<StudentEnrollmentCourseBulkRequestModel>(file);
            await _bulkService.BulkInsertStudents(records);
        }
        catch (Exception ex)
        {
            return BadRequest($"An error occurred while processing the file: {ex.Message}");
        }

        return Ok();
    }
}

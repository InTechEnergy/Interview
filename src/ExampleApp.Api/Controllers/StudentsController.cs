using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Academia;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ExampleApp.Api.Domain.Students;

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
        DateOnly today = new(2023, 9, 1);
        ICollection<Student> students = await _mediator.Send(new GetStudentsActiveOnDateQuery(today));
        _logger.LogInformation("Retrieved {Count} current students", students.Count);

        List<StudentModel> models = new();

        foreach (var student in students)
        {
            //KeyNameModel semesterModel = new KeyNameModel(student.Semester.Id, student.Semester.Description);
            //KeyNameModel professorModel = new KeyNameModel(student.Professor.Id.ToString(), student.Professor.FullName);
            StudentModel courseModel = new(student.Id, student.FullName, student.Badge, student.StudentCourses?.Count ?? 0);

            models.Add(courseModel);
        }

        return models;
    }
}

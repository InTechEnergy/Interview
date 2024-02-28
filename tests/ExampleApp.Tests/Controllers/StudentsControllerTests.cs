using ExampleApp.Api.Controllers;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia.Models;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExampleApp.Tests.Controllers;

public class StudentsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<StudentsController>> _loggerMock;
    private readonly Mock<IBulkService> _bulkServiceMock;
    private readonly Mock<ICoursesService> _coursesServiceMock;
    private readonly Mock<IFileProcessorService> _fileProcessorServiceMock;
    private readonly List<IFileProcessorService> _fileProcessors;
    private readonly Mock<IValidationsService> _validationsServiceMock;
    private readonly StudentsController _controller;

    public StudentsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<StudentsController>>();
        _bulkServiceMock = new Mock<IBulkService>();
        _coursesServiceMock = new Mock<ICoursesService>();
        _fileProcessorServiceMock = new Mock<IFileProcessorService>();
        _validationsServiceMock = new Mock<IValidationsService>();
        _fileProcessors = new() { _fileProcessorServiceMock.Object };

        _controller = new StudentsController(_mediatorMock.Object, _loggerMock.Object, _bulkServiceMock.Object, _coursesServiceMock.Object, _fileProcessors, _validationsServiceMock.Object);
    }

    [Fact]
    public async Task GetCurrent_ReturnsCorrectStudents()
    {
        List<StudentCourseCountModel> expectedStudents = new()
        {
            new (){ Student = new Student { Id = 1, FullName = "Sevann Radhak", Badge = "123", ResidentStatus = ResidentStatus.InState }, CourseCount = 2 },
            new (){ Student = new Student { Id = 2, FullName = "Dylan Rametta", Badge = "456", ResidentStatus = ResidentStatus.OutOfState }, CourseCount = 1 }
        };

        _ = _mediatorMock.Setup(m => m
            .Send(It.IsAny<GetStudentsActiveOnDateQuery>(), default))
            .ReturnsAsync(expectedStudents);

        IEnumerable<StudentModel> result = await _controller.GetCurrent();

        IEnumerable<StudentModel> students = Assert.IsAssignableFrom<IEnumerable<StudentModel>>(result);
        Assert.Equal(expectedStudents.Count, students.Count());
        Assert.Equal(expectedStudents[0].Student.Id, students.First().Id);
        Assert.Equal(expectedStudents[0].Student.FullName, students.First().fullName);
        Assert.Equal(expectedStudents[0].Student.Badge, students.First().badge);
        Assert.Equal(expectedStudents[0].CourseCount, students.First().courseCount);
    }

    [Fact]
    public async Task GetCurrent_ReturnsEmpty_WhenNoStudents()
    {
        List<StudentCourseCountModel> expectedStudents = new();

        _ = _mediatorMock.Setup(m => m
            .Send(It.IsAny<GetStudentsActiveOnDateQuery>(), default))
            .ReturnsAsync(expectedStudents);

        IEnumerable<StudentModel> result = await _controller.GetCurrent();
        IEnumerable<StudentModel> students = Assert.IsAssignableFrom<IEnumerable<StudentModel>>(result);

        Assert.Empty(students);
    }

    [Fact]
    public async Task EnrollStudentInCourse_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        _ = _validationsServiceMock
            .Setup(vs => vs.CheckErrorsAsync(It.IsAny<StudentEnrollmentCourseRequestModel>(), It.IsAny<ModelStateDictionary>()))
            .Returns(new List<ModelStateError>());

        StudentsController controller = new(_mediatorMock.Object, _loggerMock.Object, _bulkServiceMock.Object, _coursesServiceMock.Object, _fileProcessors, _validationsServiceMock.Object);
        controller.ModelState.AddModelError("TestError", "Invalid model state");
        StudentEnrollmentCourseRequestModel request = new();
        IActionResult result = await controller.EnrollStudentInCourse(request);

        _ = Assert.IsType<BadRequestObjectResult>(result);
    }
}

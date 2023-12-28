using ExampleApp.Api.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia.Commands;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp.Tests;

public class StudentControllerTests
{

    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ILogger<StudentsController>> _logger;

    public StudentControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _logger = new Mock<ILogger<StudentsController>>();
    }

    [Fact]
    public async Task MapsStudents()
    {
        // Arrange
        List<StudentModel> students = new()
        {
            new StudentModel(
                1,
                "Student one",
                1,
                "all",
                "Foreign"
            ),
            new StudentModel(
                1,
                "Student Two",
                2,
                "all",
                "Foreign"
            ),
        };

        _mediator.Setup(x => x.Send( It.IsAny<GetAllStudentsQuery>(), new CancellationToken()))
             .ReturnsAsync(students);

        // Act
        var response = await new StudentsController(_mediator.Object, _logger.Object).GetAll();

        // Assert
        Assert.True(response.Any());
        Assert.Equal(2, response.Count());
    }

    [Fact]
    public async Task RegisterInCourse_ReturnsOk()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<StudentsController>>();
        var controller = new StudentsController(mediatorMock.Object, loggerMock.Object);

        var command = new RegisterStudentInCourse
        {
            Badge = "alf",
            CourseId = "1"
        };

        mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(true);

        // Act
        var result = await controller.RegisterInCourse(command);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Registration successful", actionResult.Value);
    }

    [Fact]
    public async Task RegisterInCourse_ReturnsBadRequest()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<StudentsController>>();
        var controller = new StudentsController(mediatorMock.Object, loggerMock.Object);

        var command = new RegisterStudentInCourse
        {
            Badge = "alf",
            CourseId = "1"
        };

        mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(false);

        // Act
        var result = await controller.RegisterInCourse(command);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Registration failed", actionResult.Value);
    }

    [Fact]
    public async Task UnregisterFromCourse_ReturnsOk()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<StudentsController>>();
        var controller = new StudentsController(mediatorMock.Object, loggerMock.Object);

        var command = new UnregisterStudentInCourse
        {
            Badge = "alf",
            CourseId = "1"
        };

        mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(true);

        // Act
        var result = await controller.UnregisterFromCourse(command);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Un-registration successful", actionResult.Value);
    }

    [Fact]
    public async Task UnregisterFromCourse_ReturnsBadRequest()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<StudentsController>>();
        var controller = new StudentsController(mediatorMock.Object, loggerMock.Object);

        var command = new UnregisterStudentInCourse
        {
            Badge = "alf",
            CourseId = "1"
        };

        mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(false);

        // Act
        var result = await controller.UnregisterFromCourse(command);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Un-registration failed", actionResult.Value);
    }
}

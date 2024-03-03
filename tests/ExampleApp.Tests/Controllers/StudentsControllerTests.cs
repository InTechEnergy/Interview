using ExampleApp.Api.Controllers;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia.Models;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExampleApp.Tests.Controllers;

public class StudentsControllerTests
{
    private readonly IMediator _mediator;
    private readonly ICourseService _courseService;
    private readonly ILogger<StudentsController> _logger = Utils.CreateLogger<StudentsController>();

    public StudentsControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _courseService = Substitute.For<ICourseService>();
    }

    [Fact]
    public async Task Given_Existing_Students_When_Getting_Students_Then_Returns_Not_Empty_List()
    {
        // Arrange
        var expectedStudents = new List<StudentCourseCountModel>
        {
            new () { Student = new Student { Id = 1, FullName = "Test Student 1", Badge = "781", ResidentStatus = ResidentStatus.Foreign }, CourseCount = 2 },
            new () { Student = new Student { Id = 2, FullName = "Test Student 1", Badge = "100", ResidentStatus = ResidentStatus.InState }, CourseCount = 1 }
        };

        _mediator.Send(Arg.Any<GetStudentsQuery>(), default)
            .Returns(expectedStudents);

        // Act
        var response = await new StudentsController(_mediator, _courseService, _logger).GetCurrent();

        // Assert
        var students = Assert.IsAssignableFrom<IEnumerable<StudentModel>>(response);

        Assert.Equal(expectedStudents.Count, students.Count());

        foreach(var student in students)
        {
            var dbStudent = expectedStudents.Single(x => x.Student.Id == student.Id);

            Assert.Equal(dbStudent.Student.Id, student.Id);
            Assert.Equal(dbStudent.Student.FullName, student.fullName);
            Assert.Equal(dbStudent.Student.Badge, student.badge);
            Assert.Equal(dbStudent.CourseCount, student.courseCount);
        }        
    }

    [Fact]
    public async Task Given_No_Students_When_Getting_Students_Then_Returns_Empty_List()
    {
        // Arrange
        var expectedStudents = new List<StudentCourseCountModel>();

        _mediator.Send(Arg.Any<GetStudentsQuery>(), default)
            .Returns(expectedStudents);

        // Act
        var response = await new StudentsController(_mediator, _courseService, _logger).GetCurrent();

        // Assert
        var students = Assert.IsAssignableFrom<IEnumerable<StudentModel>>(response);
        Assert.Empty(students);
    }

    [Fact]
    public async Task Given_Empty_Course_Id_When_Assigning_Student_To_Course_Then_Returns_Bad_Request()
    {
        // Arrange
        var request = new RegisterStudentToCourseModel() { CourseId = null };

        // Act
        var response = await new StudentsController(_mediator, _courseService, _logger).RegisterStudentToCourse(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(response);
    }

    [Fact]
    public async Task Given_Empty_FullName_And_BadgeNumber_Id_When_Assigning_Student_To_Course_Then_Returns_Bad_Request()
    {
        // Arrange
        var request = new RegisterStudentToCourseModel() { CourseId = "123" };

        // Act
        var response = await new StudentsController(_mediator, _courseService, _logger).RegisterStudentToCourse(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal("FullName or BadgeNumber is required.", (response as BadRequestObjectResult)!.Value);
    }
}

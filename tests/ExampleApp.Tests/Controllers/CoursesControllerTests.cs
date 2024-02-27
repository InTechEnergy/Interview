using ExampleApp.Api.Controllers;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExampleApp.Tests.Controllers;

public class CoursesControllerTests
{
    private readonly IMediator _mediator;
    private readonly ILogger<CoursesController> _logger = Utils.CreateLogger<CoursesController>();

    public CoursesControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
    }

    [Fact]
    public async Task MapsCurrentCourses()
    {
        // Arrange
        List<Course> courses = new()
        {
            new Course(
                "test1",
                "test 1",
                new Semester("sem-1")
                {
                    Description = "sem-1",
                    Start = DateOnly.FromDateTime(DateTime.Today),
                    End = DateOnly.FromDateTime(DateTime.Today)
                },
                new Professor { FullName = "prof one" },
                DateTimeOffset.Now,
                DateTimeOffset.Now
            ),
            new Course(
                "test2",
                "test 2",
                new Semester("sem-1")
                {
                    Description = "sem-1", Start = DateOnly.FromDateTime(DateTime.Today),
                    End = DateOnly.FromDateTime(DateTime.Today)
                },
                new Professor { FullName = "prof one" },
                DateTimeOffset.Now,
                DateTimeOffset.Now
            )
        };

        _ = _mediator.Send(Arg.Any<IRequest<ICollection<Course>>>()).Returns(courses);

        // Act
        var actionResult = await new CoursesController(_mediator, _logger).GetCurrent();

        // Assert
        _ = actionResult.Should().BeOfType<OkObjectResult>();

        OkObjectResult? okResult = actionResult as OkObjectResult;
        ResponseCoursesSemesterModel? response = okResult?.Value as ResponseCoursesSemesterModel;

        _ = response.Should().NotBeNull();
        _ = (response?.Semester?.Courses.Should().HaveCount(2));
        _ = response.Should().BeEquivalentTo(
            new ResponseCoursesSemesterModel
            {
                Semester = new SemesterCoursesResponseModel("sem-1", "sem-1")
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Today),
                    EndDate = DateOnly.FromDateTime(DateTime.Today),
                    Courses = new List<CourseModel>
                    {
                        new("test1", "test 1")
                        {
                            Professor = new ProfessorModel("0", "prof one")
                        },
                        new("test2", "test 2")
                        {
                            Professor = new ProfessorModel("0", "prof one")
                        }
                    }
                }
            });
    }
}

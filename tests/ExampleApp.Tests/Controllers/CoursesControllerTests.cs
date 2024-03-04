using ExampleApp.Api.Controllers;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExampleApp.Tests;

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
                new Semester
                {
                    Description = "sem-1",
                    Start = DateOnly.FromDateTime(DateTime.Today),
                    End = DateOnly.FromDateTime(DateTime.Today)
                },
                new Professor { FullName = "prof 1" },
                DateTimeOffset.Now,
                DateTimeOffset.Now
            ),
            new Course(
                "test2",
                "test 2",
                new Semester
                {
                    Description = "sem-1", Start = DateOnly.FromDateTime(DateTime.Today),
                    End = DateOnly.FromDateTime(DateTime.Today)
                },
                new Professor { FullName = "prof 2" },
                DateTimeOffset.Now,
                DateTimeOffset.Now
            )
        };
        _mediator.Send(Arg.Any<IRequest<ICollection<Course>>>())
            .Returns(courses);

        // Act
        var response = await new CoursesController(_mediator, _logger).GetCurrent();

        // Assert
        var result = (response.Result as OkObjectResult).Value as SemesterModel;
        Assert.NotNull(result);

        result.Should()
            .BeEquivalentTo(
                    new
                    {
                        Name = "sem-1",
                        Start = DateOnly.FromDateTime(DateTime.Today),
                        End = DateOnly.FromDateTime(DateTime.Today),
                        Courses = new[]
                        {
                            new
                            {
                                Key = "test1",
                                Name = "test 1",
                                Professor = new
                                {
                                    Key = "0",
                                    Name = "prof 1"
                                }
                            },
                            new
                            {
                                Key = "test2",
                                Name = "test 2",
                                Professor = new
                                {
                                    Key = "0",
                                    Name = "prof 2"
                                }
                            }
                        }
                    });
    }
}

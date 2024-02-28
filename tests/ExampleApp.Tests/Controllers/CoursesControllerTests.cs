using System.Security.Cryptography;
using ExampleApp.Api.Controllers;
using Microsoft.Extensions.Logging;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using FluentAssertions;

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
        var courseGuid1 = Guid.NewGuid();
        var courseGuid2 = Guid.NewGuid();
        // Arrange
        List<Course> courses = new()
        {
            new Course(
                courseGuid1,
                "test 1",
                new Semester
                {
                    Description = "sem-1",
                    Start = DateOnly.FromDateTime(DateTime.Today),
                    End = DateOnly.FromDateTime(DateTime.Today)
                },
                new Lecturer { FullName = "prof one" }
            ),
            new Course(
                courseGuid2,
                "test 2",
                new Semester
                {
                    Description = "sem-2",
                    Start = DateOnly.FromDateTime(DateTime.Today),
                    End = DateOnly.FromDateTime(DateTime.Today)
                },
                new Lecturer { FullName = "prof two" }
            )
        };
        _mediator.Send(Arg.Any<IRequest<ICollection<Course>>>())
            .Returns(courses);

        // Act
        var response = await new CoursesController(_mediator, _logger).GetCurrent();

        // Assert
        response.Should().HaveCount(2);
        response.Should()
            .BeEquivalentTo(
                new[]
                {
                    new
                    {
                        Key = Guid.Empty,
                        Name = "sem-1",
                        StartDate = DateOnly.FromDateTime(DateTime.Today),
                        EndDate = DateOnly.FromDateTime(DateTime.Today),
                        Courses = new[]
                        {
                            new
                            {
                                Key = courseGuid1,
                                Name = "test 1",
                                Professor = new
                                {
                                    Key = Guid.Empty,
                                    Name = "prof one"
                                }
                            }
                        }
                    },
                    new
                    {
                        Key = Guid.Empty,
                        Name = "sem-2",
                        StartDate = DateOnly.FromDateTime(DateTime.Today),
                        EndDate = DateOnly.FromDateTime(DateTime.Today),
                        Courses = new[]
                        {
                            new
                            {
                                Key = courseGuid2,
                                Name = "test 2",
                                Professor = new
                                {
                                    Key = Guid.Empty,
                                    Name = "prof two"
                                }
                            }
                        }
                    }
                });
    }
}

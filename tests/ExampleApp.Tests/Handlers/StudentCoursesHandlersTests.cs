using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Academia.Queries.Students;
using ExampleApp.Api.Domain.Academia.QueryHandlers.Students;
using ExampleApp.Api.Domain.Academia.Seeders;
using ExampleApp.Api.Domain.SharedKernel.Contracts;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Domain.Students.Entities;
using ExampleApp.Api.Extensions;
using FluentAssertions;

namespace ExampleApp.Tests.Handlers;

public class StudentCoursesHandlersTests : IClassFixture<DatabaseFixture>, IDisposable, IAsyncDisposable
{
    private AcademiaDbContext _dbContext;
    private IMediator _mediator;
    private DatabaseFixture _fixture;

    public StudentCoursesHandlersTests(DatabaseFixture fixture)
    {
        _dbContext = fixture.Db;
        _mediator = fixture.Mediator;
        _fixture = fixture;
    }

    [Fact]
    public async Task StudentCourses_ShouldReturnCurrentCoursesAccordingTheSemester()
    {
        var courseGuidRegisteredSemester = Guid.NewGuid();

        var studentCourses = new List<StudentCourses>()
                {
                    new StudentCourses(
                        new Student("John Snow"),
                        new List<Course>()
                        {
                            new Course(
                                id: courseGuidRegisteredSemester,
                                description: "Math",
                                semester: new Semester()
                                {
                                    Description = "This is the semester",
                                    Start = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20)),
                                    End = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(20))
                                },
                                professor: new Professor()
                                {
                                    FullName = "John Snow"
                                },
                                createdOn: DateTimeOffset.Now,
                                lastModifiedOn: DateTimeOffset.Now
                            ),
                            new Course(
                                id: Guid.NewGuid(),
                                description: "Philosophy",
                                semester: new Semester()
                                {
                                    Description = "This is the semester2",
                                    Start = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)),
                                    End = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(20))
                                },
                                professor: new Professor()
                                {
                                    FullName = "Dumbledore"
                                },
                                createdOn: DateTimeOffset.Now,
                                lastModifiedOn: DateTimeOffset.Now
                            ),
                            new Course(
                                id: Guid.NewGuid(),
                                description: "History",
                                semester: new Semester()
                                {
                                    Description = "This is the semester",
                                    Start = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20)),
                                    End = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
                                },
                                professor: new Professor()
                                {
                                    FullName = "John Snow"
                                },
                                createdOn: DateTimeOffset.Now,
                                lastModifiedOn: DateTimeOffset.Now
                            )
                        }
                    )
                };

        await _dbContext.StudentCourses.AddRangeAsync(studentCourses);
        await _dbContext.SaveChangesAsync();

        var result = await _mediator.Send(new GetStudentCoursesByCurrentSemesterQuery());

        var studentCourseViews = result.Value;
        var courseIds = studentCourseViews.Select(x => x.CourseIds);

        // Assert
        result
            .IsSuccess
            .Should()
            .BeTrue();

        studentCourseViews.Should().HaveCount(1);

        courseIds.Should().HaveCount(1);
        courseIds.FirstOrDefault().Should().Contain(courseGuidRegisteredSemester);
    }

    public void Dispose()
    {
        _fixture.DisposeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}

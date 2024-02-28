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

public class StudentCoursesHandlersTests : IClassFixture<TestApplication>
{
    private TestApplication _testApplication;

    public StudentCoursesHandlersTests(TestApplication testApplication)
    {
        _testApplication = testApplication;
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
                                lecturer: new Lecturer()
                                {
                                    FullName = "John Snow"
                                }
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
                                lecturer: new Lecturer()
                                {
                                    FullName = "Dumbledore"
                                }
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
                                lecturer: new Lecturer()
                                {
                                    FullName = "John Snow"
                                }
                            )
                        }
                    )
                };

        await _testApplication.DbContext.StudentCourses.AddRangeAsync(studentCourses);
        await _testApplication.DbContext.SaveChangesAsync();

        var result = await _testApplication.Mediator.Send(new GetStudentCoursesByCurrentSemesterQuery());

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
}

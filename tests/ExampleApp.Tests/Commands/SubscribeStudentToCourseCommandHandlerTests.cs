using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Domain.Students.Entities;
using ExampleApp.Api.Utils.Exceptions;
using FluentAssertions;

namespace ExampleApp.Tests.Commands;

[Collection(nameof(TestApplicationCollection))]
public class SubscribeStudentToCourseCommandHandlerTests : IClassFixture<BaseTest>
{
    private readonly TestApplication _testApplication;

    public SubscribeStudentToCourseCommandHandlerTests(TestApplication testApplication)
    {
        _testApplication = testApplication;
    }

    [Fact]
    public async void CourseStudent_WhenStudentNotFound_ShouldThrowError()
    {
        // Arrange
        var command = new SubscribeStudentToCourseCommand(new StudentToCourseModel(Guid.NewGuid(), new StudentModel("fake name", 1)) );

        await FluentActions.Awaiting(() => _testApplication.Mediator.Send(command))
            .Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async void CourseStudent_WhenCourseNotFound_ShouldThrowError()
    {
        _testApplication.DbContext.Students.Add(new Student("New student 1"));

        await _testApplication.DbContext.SaveChangesAsync();

        var command = new SubscribeStudentToCourseCommand(new StudentToCourseModel(Guid.NewGuid(), new StudentModel("New student 1", 1)) );

        await FluentActions.Awaiting(() => _testApplication.Mediator.Send(command))
            .Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async void CourseStudent_ShouldSubscribeStudentToCourse()
    {
        var courseId = Guid.NewGuid();

        var student = _testApplication.DbContext.Students.Add(new Student("New student 1"));

        _testApplication.DbContext.Courses.Add(
            new Course(
                id: courseId,
                description: "Philosophy",
                semester: new Semester()
                {
                    Description = "this is the semester",
                    Start = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)),
                    End = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(20))
                },
                professor: new Professor()
                {
                    FullName = "Professor Math"
                }));

        await _testApplication.DbContext.SaveChangesAsync();

        var command = new SubscribeStudentToCourseCommand(new StudentToCourseModel(courseId, new StudentModel("New student 1", 1)) );

        var (response, _, _) = await _testApplication.Mediator.Send(command);

        var course = _testApplication.DbContext.StudentCourses.SingleOrDefault(x => x.StudentId == student.Entity.Id);

        response.Should().BeTrue();

        course.Should().NotBeNull();
    }

    [Fact]
    public async void CourseStudent_WhenCourseIsNotCurrent_ShouldThrowException()
    {
        var courseId = Guid.NewGuid();

        var student = _testApplication.DbContext.Students.Add(new Student("New student 1"));

        _testApplication.DbContext.Courses.Add(
            new Course(
                id: courseId,
                description: "Philosophy",
                semester: new Semester()
                {
                    Description = "this is the semester",
                    Start = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)),
                    End = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(20))
                },
                professor: new Professor()
                {
                    FullName = "Professor Math"
                }));

        await _testApplication.DbContext.SaveChangesAsync();

        var command = new SubscribeStudentToCourseCommand(new StudentToCourseModel(courseId, new StudentModel("New student 1", 1)) );

        await FluentActions.Awaiting(() => _testApplication.Mediator.Send(command))
            .Should()
            .ThrowAsync<BusinessException>();
    }
}

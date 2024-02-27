using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Domain.Students.Entities;
using ExampleApp.Api.Utils.Exceptions;
using FluentAssertions;

namespace ExampleApp.Tests.Commands;

public class SubscribeStudentToCourseCommandHandlerTests : IClassFixture<DatabaseFixture>
{
    private readonly AcademiaDbContext _dbContext;
    private readonly IMediator _mediator;

    public SubscribeStudentToCourseCommandHandlerTests(DatabaseFixture fixture)
    {
        _dbContext = fixture.Db;
        _mediator = fixture.Mediator;
    }

    [Fact]
    public async void CourseStudent_WhenStudentNotFound_ShouldThrowError()
    {
        // Arrange
        var command = new SubscribeStudentToCourseCommand(new SubscribeStudentToCourseModel(Guid.NewGuid(), new StudentModel("fake name", 1)) );

        await FluentActions.Awaiting(() => _mediator.Send(command))
            .Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async void CourseStudent_WhenCourseNotFound_ShouldThrowError()
    {
        _dbContext.Students.Add(new Student("New student 1"));

        await _dbContext.SaveChangesAsync();

        var command = new SubscribeStudentToCourseCommand(new SubscribeStudentToCourseModel(Guid.NewGuid(), new StudentModel("New student 1", 1)) );

        await FluentActions.Awaiting(() => _mediator.Send(command))
            .Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async void CourseStudent_ShouldSubscribeStudentToCourse()
    {
        var courseId = Guid.NewGuid();

        var student = _dbContext.Students.Add(new Student("New student 1"));

        _dbContext.Courses.Add(
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
                },
                DateTimeOffset.Now,
                DateTimeOffset.Now));

        await _dbContext.SaveChangesAsync();

        var command = new SubscribeStudentToCourseCommand(new SubscribeStudentToCourseModel(courseId, new StudentModel("New student 1", 1)) );

        var (response, _, _) = await _mediator.Send(command);

        var course = _dbContext.StudentCourses.SingleOrDefault(x => x.StudentId == student.Entity.Id);

        response.Should().BeTrue();

        course.Should().NotBeNull();
    }

    [Fact]
    public async void CourseStudent_WhenCourseIsNotCurrent_ShouldThrowException()
    {
        var courseId = Guid.NewGuid();

        var student = _dbContext.Students.Add(new Student("New student 1"));

        _dbContext.Courses.Add(
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
                },
                DateTimeOffset.Now,
                DateTimeOffset.Now));

        await _dbContext.SaveChangesAsync();

        var command = new SubscribeStudentToCourseCommand(new SubscribeStudentToCourseModel(courseId, new StudentModel("New student 1", 1)) );

        await FluentActions.Awaiting(() => _mediator.Send(command))
            .Should()
            .ThrowAsync<BusinessException>();
    }
}

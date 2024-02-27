using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Domain.Students.Entities;
using FluentAssertions;

namespace ExampleApp.Tests.Commands;

public class UnsubscribeStudentToCourseCommandHandlerTests : IClassFixture<DatabaseFixture>
{
    private readonly AcademiaDbContext _dbContext;
    private readonly IMediator _mediator;

    public UnsubscribeStudentToCourseCommandHandlerTests(DatabaseFixture fixture)
    {
        _dbContext = fixture.Db;
        _mediator = fixture.Mediator;
    }

    [Fact]
    public async void CourseStudent_ShouldUnsubscribeStudentToCourse()
    {
        var courseId = Guid.NewGuid();

        var student = _dbContext.Students.Add(new Student("New student 1"));

        var courseEntity = _dbContext.Courses.Add(
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

        _dbContext.StudentCourses.Add(
            new StudentCourses(
                student.Entity,
                new List<Course>()
                {
                    courseEntity.Entity
                }));

        await _dbContext.SaveChangesAsync();

        var command = new UnsubscribeStudentToCourseCommand(new StudentToCourseModel(courseId, new StudentModel("New student 1", 1)) );

        var (response, _, _) = await _mediator.Send(command);

        var course = _dbContext.StudentCourses.SingleOrDefault(x => x.StudentId == student.Entity.Id);

        response.Should().BeTrue();

        course.Courses.FirstOrDefault().IsDeleted.Should().BeTrue();
    }
}

using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Domain.Students.Entities;
using FluentAssertions;

namespace ExampleApp.Tests.Commands;

public class UnsubscribeStudentToCourseCommandHandlerTests : IClassFixture<TestApplication>, IDisposable
{
    private readonly TestApplication _testApplication;

    public UnsubscribeStudentToCourseCommandHandlerTests(TestApplication testApplication)
    {
        _testApplication = testApplication;
    }

    [Fact]
    public async void CourseStudent_ShouldUnsubscribeStudentToCourse()
    {
        var courseId = Guid.NewGuid();

        var student = _testApplication.DbContext.Students.Add(new Student("New student 1"));

        var courseEntity = _testApplication.DbContext.Courses.Add(
            new Course(
                id: courseId,
                description: "Philosophy",
                semester: new Semester()
                {
                    Description = "this is the semester",
                    Start = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)),
                    End = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(20))
                },
                lecturer: new Lecturer()
                {
                    FullName = "Professor Math"
                }));

        _testApplication.DbContext.StudentCourses.Add(
            new StudentCourses(
                student.Entity,
                new List<Course>()
                {
                    courseEntity.Entity
                }));

        await _testApplication.DbContext.SaveChangesAsync();

        var command = new UnsubscribeStudentToCourseCommand(new StudentToCourseModel(courseId, new StudentModel("New student 1", 1)) );

        var (response, _, _) = await _testApplication.Mediator.Send(command);

        var course = _testApplication.DbContext.StudentCourses.SingleOrDefault(x => x.StudentId == student.Entity.Id);

        response.Should().BeTrue();

        course.Courses.FirstOrDefault().IsDeleted.Should().BeTrue();
    }

    public void Dispose()
    {
        _testApplication.ResetDatabase();
    }
}

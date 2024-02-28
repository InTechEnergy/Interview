using ExampleApp.Api.Controllers;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExampleApp.Tests;

public sealed class CourseControllerIntegrationTests : IClassFixture<TestApplication>
{
    private AcademiaDbContext _db;
    private CoursesController _controller;
    private Course _course;
    private TestApplication _fixture;

    public CourseControllerIntegrationTests(TestApplication testApplication)
    {
        _fixture = testApplication;
        var mediator = (IMediator)testApplication.Services.GetService(typeof(IMediator))!;
        var logger = (ILogger<CoursesController>)testApplication.Services.GetService(typeof(ILogger<CoursesController>))!;
        _controller = new CoursesController(mediator, logger);
        _db = (AcademiaDbContext)testApplication.Services.GetService(typeof(AcademiaDbContext))!;

        var semester = _db.Semesters.Add(new Semester()
        {
            Description = "Test Semester 01",
            Start = new DateOnly(1999, 1, 1),
            End = new DateOnly(1999, 12, 1)
        });

        var professor = new Professor { FullName = "test professor 01" };

        _db.Professors.AddRange( professor, new Professor { FullName = "test professor 02", Extension = "02" });

        _course = _db.Courses.Add(new Course(Guid.NewGuid(), "Test Course 01", professor: professor, semester: semester.Entity)).Entity;

        _db.SaveChanges();
    }

    [Fact]
    public async Task UpdatesProfessor()
    {
        // Arrange
        ProfessorUpdateModel payload = new(_course.Id, "test professor 02");

        // Act
        var response = await _controller.UpdateProfessor(payload);

        // Assert
        response.Should().BeOfType<AcceptedResult>();

        Course? course = await _db.Courses
            .Include(c => c.Professor)
            .Include(c => c.Semester)
            .FirstOrDefaultAsync(c => c.Id == payload.CourseId);

        course.Should().NotBeNull();
        course.Professor.FullName.Should().Be("test professor 02");
        course.Professor.Extension.Should().Be("02");
    }

    [Fact]
    public async Task ReturnsNotFoundIfCourseIsNotFound()
    {
        // Arrange
        ProfessorUpdateModel payload = new(Guid.NewGuid(), "test professor 02");

        // Act
        var response = await _controller.UpdateProfessor(payload);

        // Assert
        response.Should()
            .NotBeNull()
            .And.BeOfType<NotFoundObjectResult>();

        string? message = ((NotFoundObjectResult)response).Value as string;

        message.Should()
            .NotBeNull()
            .And.Contain("Invalid course");

        Course? course = await _db.Courses
            .Include(c => c.Professor)
            .Include(c => c.Semester)
            .FirstOrDefaultAsync(c => c.Id == _course.Id);

        course.Should().NotBeNull();
        course.Professor.FullName.Should().Be("test professor 01");
        course.Professor.Extension.Should().BeNull();
    }
}

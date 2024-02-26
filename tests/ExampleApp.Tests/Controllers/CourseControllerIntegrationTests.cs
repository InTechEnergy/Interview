using ExampleApp.Api.Controllers;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExampleApp.Tests;

public class CourseControllerIntegrationTests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly AcademiaDbContext _db;
    private readonly CoursesController _controller;

    public CourseControllerIntegrationTests(DatabaseFixture fixture)
    {
        var mediator = (IMediator)fixture.Services.GetService(typeof(IMediator))!;
        var logger = (ILogger<CoursesController>)fixture.Services.GetService(typeof(ILogger<CoursesController>))!;
        _controller = new CoursesController(mediator, logger);
        _db = (AcademiaDbContext)fixture.Services.GetService(typeof(AcademiaDbContext))!;

        // remove previous values, in case there were left by an aborted debug session
        Dispose();

        // create records available for all tests
        _db.Database.ExecuteSql(
            $@"INSERT INTO [example-db].dbo.Semesters
  (Id, Description, StartDate, EndDate)
  VALUES
    ('tst-01', 'Test Semester 01', '1999-01-01', '1999-12-01');
INSERT INTO [example-db].dbo.Professors
  (FullName, Extension)
  VALUES
    ('test professor 01', null),
    ('test professor 02', '02');
INSERT INTO [example-db].dbo.Courses
  (Id, Description, ProfessorId, SemesterId)
SELECT
    'TEST-01' [Id],
    'Test Course 01' [Description],
    p.Id ProfessorId,
    'tst-01' SemesterId
FROM [example-db].dbo.Professors p
WHERE FullName = 'test professor 01';
");
    }

    public void Dispose()
    {
        // clean up after tests
        _db.Database.ExecuteSql(
            $@"
DELETE FROM [example-db].dbo.Courses WHERE Id = 'TEST-01';
DELETE FROM [example-db].dbo.Professors WHERE FullName like 'test professor 0%';
DELETE FROM [example-db].dbo.Semesters WHERE Id = 'tst-01';
            ");
    }

    [Fact]
    public async Task UpdatesProfessor()
    {
        // Arrange
        ProfessorUpdateModel payload = new(Guid.NewGuid(), "test professor 02");

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
            .And.Be("Invalid course TEST-00");

        Course? course = await _db.Courses
            .Include(c => c.Professor)
            .Include(c => c.Semester)
            .FirstOrDefaultAsync(c => c.Id == payload.CourseId);

        course.Should().NotBeNull();
        course.Professor.FullName.Should().Be("test professor 01");
        course.Professor.Extension.Should().BeNull();
    }
}

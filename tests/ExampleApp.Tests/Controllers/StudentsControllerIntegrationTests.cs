using ExampleApp.Api.Controllers;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExampleApp.Tests.Controllers;

public class StudentsControllerIntegrationTests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly AcademiaDbContext _db;
    private readonly StudentsController _controller;

    public StudentsControllerIntegrationTests(DatabaseFixture fixture)
    {
        var mediator = (IMediator)fixture.Services.GetService(typeof(IMediator))!;
        var logger = (ILogger<StudentsController>)fixture.Services.GetService(typeof(ILogger<StudentsController>))!;
        _controller = new StudentsController(mediator, logger);
        _db = (AcademiaDbContext)fixture.Services.GetService(typeof(AcademiaDbContext))!;

        Dispose();
    }

    public void Dispose()
    {
    }

    [Fact]
    public async Task GetsCurrentStudents()
    {
        // Arrange
        var expectedStudentCount = await _db.Students.CountAsync();


        // Act
        IEnumerable<StudentModel> response = await _controller.GetCurrent();


        // Assert
        response.Should().NotBeNull();
        response.Count().Should().Be(expectedStudentCount);
    }
}

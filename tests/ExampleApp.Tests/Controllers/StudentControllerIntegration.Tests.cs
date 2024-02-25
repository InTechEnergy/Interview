using ExampleApp.Api.Controllers;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
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

        // create records available for all tests
        // TODO: Insert test students and their courses into the database
    }

    public void Dispose()
    {
        // clean up after tests
        // TODO: Delete test students and their courses from the database
    }

    [Fact]
    public async Task GetsCurrentStudents()
    {
        int expectedStudentCount = await _db.Students.CountAsync();

        IEnumerable<StudentModel> response = await _controller.GetCurrent();

        _ = response.Should().NotBeNull();
        _ = response.Count().Should().Be(expectedStudentCount);

        var students = response.ToList();

        for (int i = 0; i < students.Count - 1; i++)
        {
            if (!(students[i].courseCount == students[i + 1].courseCount))
            {
                _ = students[i].courseCount.Should().BeGreaterThan(students[i + 1].courseCount);
            }
        }
    }
}

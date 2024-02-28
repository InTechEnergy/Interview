using ExampleApp.Api.Controllers;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Interfaces;
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
        var bulkService = (IBulkService)fixture.Services.GetService(typeof(IBulkService))!;
        var coursesService = (ICoursesService)fixture.Services.GetService(typeof(ICoursesService))!;
        var fileprocessorService = (IFileProcessorService)fixture.Services.GetService(typeof(IFileProcessorService))!;
        var validationsService = (IValidationsService)fixture.Services.GetService(typeof(IValidationsService))!;

        List<IFileProcessorService> fileProcessors = new() { fileprocessorService };

        _controller = new StudentsController(mediator, logger, bulkService, coursesService, fileProcessors, validationsService);
        _db = (AcademiaDbContext)fixture.Services.GetService(typeof(AcademiaDbContext))!;

        ((IDisposable)this).Dispose();
    }

    void IDisposable.Dispose()
    {
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

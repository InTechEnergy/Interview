using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Domain.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia.Seeders;

internal class DatabaseSeeder
{
    private readonly AcademiaDbContext _academiaDbContext;

    public DatabaseSeeder(AcademiaDbContext academiaDbContext)
    {
        _academiaDbContext = academiaDbContext;
    }

    public async Task Seed()
    {
        var professors = new List<Lecturer>()
        {
            new()
            {
                FullName = "John"
            },
            new()
            {
                FullName = "Jane"
            },
            new()
            {
                FullName = "Jack"
            }
        };

        await _academiaDbContext.Professors.AddRangeAsync(professors);

        var semesters = new List<Semester>()
        {
            new Semester()
            {
                Description = "Semester 01",
                Start = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)),
                End = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(20))
            },
            new Semester()
            {
                Description = "Semester 02",
                Start = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20)),
                End = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(20))
            },
            new Semester()
            {
                Description = "Semester 03",
                Start = DateOnly.FromDateTime(DateTime.UtcNow),
                End = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5))
            }
        };

        await _academiaDbContext.Semesters.AddRangeAsync(semesters);

        var course01 = new Course(Guid.NewGuid(), "Physics 101", semesters[0], professors[0]);
        var course02 = new Course(Guid.NewGuid(), "Math 101", semesters[1], professors[1]);
        var course03 = new Course(Guid.NewGuid(), "Math 101", semesters[2], professors[2]);

        await _academiaDbContext.Courses.AddRangeAsync(course01, course02, course03);

        await _academiaDbContext.StudentCourses.AddAsync((new StudentCourses(new Student("jon"), new List<Course>(){course01, course02, course03})));

        await _academiaDbContext.SaveChangesAsync();
    }
}

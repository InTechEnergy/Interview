using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.SharedKernel.Contracts;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Domain.SharedKernel.Specifications;
using ExampleApp.Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Infrastructure.Repositories;

internal sealed class StudentCoursesRepository : IStudentCoursesRepository
{
    private readonly AcademiaDbContext _dbContext;
    private readonly ISpecificationEvaluator _specificationEvaluator;

    public StudentCoursesRepository(
        AcademiaDbContext dbContext,
        ISpecificationEvaluator specificationEvaluator)
    {
        _dbContext = dbContext;
        _specificationEvaluator = specificationEvaluator;
    }

    public async Task<List<StudentCourses>> GetAllByCurrentSemesterAsync()
    {
        var currentCourseSpecification = new CurrentCoursesSpecification();

        var dateNow = DateOnly.FromDateTime(DateTime.UtcNow);

        var query = _dbContext
            .StudentCourses
            .Include(sc => sc.Courses)
            .ThenInclude(c => c.Semester);

        return await _specificationEvaluator
            .GetQuery(query, currentCourseSpecification)
            .AsSplitQuery()
            .Select(x => new StudentCourses(x.Student, x.Courses
                .Where(c => c.Semester.Start <= dateNow && c.Semester.End >= dateNow)
                .ToList()))
            .ToListAsync();
    }
}

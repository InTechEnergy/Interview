using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.SharedKernel.Contracts;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Domain.SharedKernel.Specifications;
using ExampleApp.Api.Domain.Students.Entities;
using ExampleApp.Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Infrastructure.Repositories;

internal sealed class CoursesRepository : ICoursesRepository
{
    private readonly AcademiaDbContext _dbContext;
    private readonly ISpecificationEvaluator _specificationEvaluator;

    public CoursesRepository(
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
            .Include(sc => sc.Courses
                .Where(c => !c.IsDeleted))
            .ThenInclude(c => c.Semester);

        return await _specificationEvaluator
            .GetQuery(query, currentCourseSpecification)
            .AsSplitQuery()
            .Select(x => new StudentCourses(x.Student, x.Courses
                .Where(c => c.Semester.Start <= dateNow && c.Semester.End >= dateNow)
                .ToList()))
            .ToListAsync();
    }

    public Task<Course> GetCourseByIdAsync(Guid courseId) => _dbContext.Courses
        .Include(c => c.Professor)
        .Include(c => c.Semester)
        .SingleOrDefaultAsync(c => c.Id == courseId && !c.IsDeleted);

    public async Task UnsubscribeStudentFromCourseAsync(Course course, Student student)
    {
        var studentCourses = await _dbContext.StudentCourses
            .Include(sc => sc.Courses)
                .ThenInclude(c => c.Semester)
            .SingleOrDefaultAsync(sc => sc.Student.Id == student.Id && sc.Courses.Any(c => c.Id == course.Id));

        if (studentCourses is null)
            return;

        studentCourses.Courses.FirstOrDefault(x => x.Id == course.Id).IsDeleted = true;
    }

    public async Task<StudentCourses> SubscribeStudentToCourseAsync(Course course, Student student)
    {
        var studentCourses = new StudentCourses(student, new List<Course> { course });
        await _dbContext.StudentCourses.AddAsync(studentCourses);

        return studentCourses;
    }

    public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
}

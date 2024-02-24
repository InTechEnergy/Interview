using ExampleApp.Api.Domain.Academia.Models;
using ExampleApp.Api.Domain.Academia.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers;

internal class GetStudentsActiveOnDateHandler : IRequestHandler<GetStudentsActiveOnDateQuery, ICollection<StudentCourseCountModel>>
{
    private readonly AcademiaDbContext _context;

    public GetStudentsActiveOnDateHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<StudentCourseCountModel>> Handle(GetStudentsActiveOnDateQuery request, CancellationToken cancellationToken)
    {
        return await _context.Students
            .Include(sc => sc.StudentCourses)
                .ThenInclude(c => c.Course)
                .ThenInclude(s => s.Semester)
            .Select(s => new StudentCourseCountModel
            {
                Student = s,
                CourseCount = s.StudentCourses.Count(sc => sc.Course.Semester.Start <= request.ActiveOn && request.ActiveOn <= sc.Course.Semester.End)
            })
            .ToListAsync(cancellationToken);
    }
}

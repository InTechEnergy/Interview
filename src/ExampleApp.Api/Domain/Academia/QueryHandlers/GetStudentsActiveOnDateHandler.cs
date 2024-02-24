using ExampleApp.Api.Domain.Academia.Models;
using ExampleApp.Api.Domain.Academia.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers;

internal class GetStudentsActiveOnDateHandler : IRequestHandler<GetStudentsActiveOnDateQuery, ICollection<StudentCourseCount>>
{
    private readonly AcademiaDbContext _context;

    public GetStudentsActiveOnDateHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<StudentCourseCount>> Handle(GetStudentsActiveOnDateQuery request, CancellationToken cancellationToken)
    {
        return await _context.Students
            .Include(sc => sc.StudentCourses)
                .ThenInclude(c => c.Course)
                .ThenInclude(s => s.Semester)
            .Select(s => new StudentCourseCount
            {
                Student = s,
                CourseCount = s.StudentCourses.Count(sc => sc.Course.Semester.Start <= request.ActiveOn && request.ActiveOn <= sc.Course.Semester.End)
            })
            .ToListAsync(cancellationToken);


        //.ToListAsync(cancellationToken: cancellationToken)

        //.ContinueWith(t => t.Result
        //    .Where(s => s.StudentCourses != null && s.StudentCourses
        //        && s.StudentCourses.Course != null && s.StudentCourses.Course.Semester != null

        //    .Count(sc => sc?.Course?
        //        .Semester.Start <= request.ActiveOn && request.ActiveOn <= sc.Course.Semester.End) > 0
        //        )
        //    .ToList(), cancellationToken);
    }
}

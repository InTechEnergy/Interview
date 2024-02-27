using ExampleApp.Api.Domain.Academia.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers;

internal class GetCoursesActiveOnDateHandler : IRequestHandler<GetCoursesActiveOnDateQuery, ICollection<Course>>
{
    private readonly AcademiaDbContext _context;

    public GetCoursesActiveOnDateHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Course>> Handle(GetCoursesActiveOnDateQuery request, CancellationToken cancellationToken)
    {
        return await _context.Courses
            .Where(c => c.Semester.Start <= request.ActiveOn && request.ActiveOn <= c.Semester.End)
            .Include(c => c.Semester)
            .Include(c => c.Lecturer)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}

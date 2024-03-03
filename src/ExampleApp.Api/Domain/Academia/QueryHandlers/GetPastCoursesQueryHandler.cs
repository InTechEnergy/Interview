using ExampleApp.Api.Domain.Academia.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers;

internal class GetPastCoursesQueryHandler : IRequestHandler<GetPastCoursesQuery, ICollection<Course>>
{
    private readonly AcademiaDbContext _context;

    public GetPastCoursesQueryHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Course>> Handle(GetPastCoursesQuery request, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var courses = await _context.Courses
            .Where(c => c.Semester.End < today)
            .Include(c => c.Semester)
            .Include(c => c.Lecturer)
            .ToListAsync(cancellationToken: cancellationToken);
        return courses;
    }
}

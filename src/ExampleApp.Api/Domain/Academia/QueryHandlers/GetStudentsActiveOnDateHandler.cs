using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers;

internal class GetStudentsActiveOnDateHandler : IRequestHandler<GetStudentsActiveOnDateQuery, ICollection<Student>>
{
    private readonly AcademiaDbContext _context;

    public GetStudentsActiveOnDateHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Student>> Handle(GetStudentsActiveOnDateQuery request, CancellationToken cancellationToken)
    {
        return await _context.Students
            .Include(c => c.StudentCourses)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}

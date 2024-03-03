using ExampleApp.Api.Domain.Students.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Students.QueryHandlers;

internal class FindStudentQueryHandler : IRequestHandler<FindStudentQuery, Student?>
{
    private readonly AcademiaDbContext _context;

    public FindStudentQueryHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<Student?> Handle(FindStudentQuery request, CancellationToken cancellationToken)
    {
        var studentsQuery = _context.Students.AsQueryable();

        if (!string.IsNullOrEmpty(request.fullName))
        {
            studentsQuery = studentsQuery.Where(c => c.FullName.ToLower() == request.fullName.ToLower());
        }
        else if (!string.IsNullOrEmpty(request.badgeNumber))
        {
            studentsQuery = studentsQuery.Where(c => c.Badge.ToLower() == request.badgeNumber.ToLower());
        }

        return await studentsQuery.FirstOrDefaultAsync(cancellationToken);
    }
}

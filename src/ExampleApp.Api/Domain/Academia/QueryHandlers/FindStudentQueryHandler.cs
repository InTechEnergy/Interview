using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers;

internal class FindStudentQueryHandler : IRequestHandler<FindStudentQuery, Student?>
{
    private readonly AcademiaDbContext _context;

    public FindStudentQueryHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<Student?> Handle(FindStudentQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Student> query = _context.Students.AsQueryable();

        if (!string.IsNullOrEmpty(request.request.FullName))
        {
            query = query.Where(c => c.FullName == request.request.FullName);
        }

        if (!string.IsNullOrEmpty(request.request.BadgeNumber))
        {
            query = query.Where(c => c.Badge == request.request.BadgeNumber);
        }

        return await query.FirstOrDefaultAsync(cancellationToken);

        //return await _context.Students
        //        .FirstOrDefaultAsync(c => c.FullName == request.request.FullName
        //            || c.Badge == request.request.BadgeNumber
        //        , cancellationToken);
    }
}

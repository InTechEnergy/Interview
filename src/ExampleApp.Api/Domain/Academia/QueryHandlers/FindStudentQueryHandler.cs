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
            query = query.Where(c => c.FullName.ToLower() == request.request.FullName.ToLower());
        }

        if (!string.IsNullOrEmpty(request.request.BadgeNumber))
        {
            query = query.Where(c => c.Badge.ToLower() == request.request.BadgeNumber.ToLower());
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }
}

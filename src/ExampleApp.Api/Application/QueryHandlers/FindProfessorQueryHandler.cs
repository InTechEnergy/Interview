using ExampleApp.Api.Domain.Academia.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers;

internal class FindProfessorQueryHandler : IRequestHandler<FindProfessorByNamedQuery, Lecturer?>
{
    private readonly AcademiaDbContext _context;

    public FindProfessorQueryHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<Lecturer?> Handle(FindProfessorByNamedQuery request, CancellationToken cancellationToken)
        => await _context.Professors
            .SingleOrDefaultAsync(c => c.FullName == request.Name, cancellationToken);
}

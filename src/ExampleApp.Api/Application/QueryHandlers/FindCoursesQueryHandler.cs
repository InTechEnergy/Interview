using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers;

internal class FindCoursesQueryHandler : IRequestHandler<FindCourseByIdQuery, Course?>
{
    private readonly AcademiaDbContext _context;

    public FindCoursesQueryHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<Course?> Handle(FindCourseByIdQuery request, CancellationToken cancellationToken)
        => await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
}

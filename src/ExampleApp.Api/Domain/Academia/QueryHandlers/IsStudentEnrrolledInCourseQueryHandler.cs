using ExampleApp.Api.Domain.Academia.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers;

internal class IsStudentEnrrolledInCourseQueryHandler : IRequestHandler<IsStudentEnrolledInCourseQuery, bool>
{
    private readonly AcademiaDbContext _context;

    public IsStudentEnrrolledInCourseQueryHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(IsStudentEnrolledInCourseQuery request, CancellationToken cancellationToken)
        => await _context.StudentCourses
        .AnyAsync(x => x.CourseId == request.courseId && x.StudentId == request.studentId, cancellationToken: cancellationToken);
}

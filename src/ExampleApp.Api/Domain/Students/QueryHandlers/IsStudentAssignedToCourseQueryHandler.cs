using ExampleApp.Api.Domain.Students.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Students.QueryHandlers;

internal class IsStudentAssignedToCourseQueryHandler : IRequestHandler<IsStudentAssignedToCourseQuery, bool>
{
    private readonly AcademiaDbContext _context;

    public IsStudentAssignedToCourseQueryHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(IsStudentAssignedToCourseQuery request, CancellationToken cancellationToken)
        => await _context.StudentCourses
            .AnyAsync(x => x.CourseId == request.courseId && x.StudentId == request.studentId, cancellationToken: cancellationToken);
}

using ExampleApp.Api.Domain.Students.Commands;
using MediatR;

namespace ExampleApp.Api.Domain.Students.CommandHanlders;

internal class UnregisterStudentFromCourseCommandHandler : IRequestHandler<UnregisterStudentFromCourseCommand, Unit>
{
    private readonly AcademiaDbContext _context;

    public UnregisterStudentFromCourseCommandHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UnregisterStudentFromCourseCommand request, CancellationToken cancellationToken)
    {
        var studentCourse = _context.StudentCourses.First(x => x.CourseId == request.CourseId && x.StudentId == request.StudentId);
        _context.StudentCourses.Remove(studentCourse);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

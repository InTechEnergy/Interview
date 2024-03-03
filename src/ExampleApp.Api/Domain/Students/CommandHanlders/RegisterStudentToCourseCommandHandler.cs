using ExampleApp.Api.Domain.Students.Commands;
using MediatR;

namespace ExampleApp.Api.Domain.Students.CommandHanlders;

internal class RegisterStudentToCourseCommandHandler : IRequestHandler<RegisterStudentToCourseCommand, Unit>
{
    private readonly AcademiaDbContext _context;

    public RegisterStudentToCourseCommandHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RegisterStudentToCourseCommand request, CancellationToken cancellationToken)
    {
        var studentCourse = new StudentCourse
        {
            CourseId = request.courseId,
            StudentId = request.studentId
        };

        await _context.StudentCourses.AddAsync(studentCourse, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

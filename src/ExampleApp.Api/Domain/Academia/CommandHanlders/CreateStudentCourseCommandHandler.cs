using ExampleApp.Api.Domain.Academia.Commands;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.CommandHanlders;

internal class CreateStudentCourseCommandHandler : IRequestHandler<CreateStudentCourse, Unit>
{
    private readonly AcademiaDbContext _context;

    public CreateStudentCourseCommandHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateStudentCourse request, CancellationToken cancellationToken)
    {
        _ = await _context.StudentCourses.AddAsync(request.ToStudentCourse(), cancellationToken);
        _ = await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

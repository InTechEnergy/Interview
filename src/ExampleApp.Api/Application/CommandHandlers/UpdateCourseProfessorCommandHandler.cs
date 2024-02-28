using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Academia.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Application.CommandHandlers;

internal class UpdateCourseProfessorCommandHandler : IRequestHandler<UpdateCourseProfessor, Unit>
{
    private readonly AcademiaDbContext _context;
    private readonly ILogger<UpdateCourseProfessorCommandHandler> _logger;

    public UpdateCourseProfessorCommandHandler(
        AcademiaDbContext context,
        ILogger<UpdateCourseProfessorCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateCourseProfessor request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .Include(c => c.Lecturer)
            .Include(c => c.Semester)
            .SingleAsync(c => c.Id == request.CourseId, cancellationToken: cancellationToken);

        var newProfessor = await _context.Professors.SingleAsync(
            p => p.Id == request.NewProfessorId,
            cancellationToken: cancellationToken);

        if (course.Lecturer.Id == newProfessor.Id)
        {
            _logger.LogInformation(
                "Course's new professor is the same as the current (id={Id}); nothing to update",
                course.Lecturer.Id);
            return Unit.Value;
        }

        course.UpdateProfessor(newProfessor);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.SharedKernel.Contracts;
using ExampleApp.Api.Domain.Students.Contracts;
using ExampleApp.Api.Utils.Exceptions;
using ExampleApp.Api.Utils.Models;
using MediatR;

namespace ExampleApp.Api.Application.CommandHandlers;

internal class UnsubscribeStudentToCourseCommandHandler  : IRequestHandler<UnsubscribeStudentToCourseCommand, Result<Unit>>
{
    private readonly ICoursesRepository _coursesRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<UnsubscribeStudentToCourseCommand> _logger;

    public UnsubscribeStudentToCourseCommandHandler(
        ICoursesRepository coursesRepository,
        IStudentRepository studentRepository,
        ILogger<UnsubscribeStudentToCourseCommand> logger)
    {
        _coursesRepository = coursesRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(UnsubscribeStudentToCourseCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Unsubscribing student from course.");

        if (request is { StudentCourse: null or { Student: null } })
            throw new BusinessException("InvalidRequest", "Invalid request.");

        var studentFullName = request.StudentCourse.Student.FullName;
        var studentBadge = request.StudentCourse.Student.Badge;
        var courseId = request.StudentCourse.CourseId;

        _logger.LogInformation("Starting getting student and course.");

        var student = await _studentRepository.GetByNameOrBadgeAsync(studentFullName, studentBadge);

        var course = await _coursesRepository.GetCourseByIdAsync(courseId);

        if (student is null || course is null)
            throw new EntityNotFoundException();

        if (!course.IsCurrentOnSemester())
            throw new BusinessException("SemesterNotCurrent", "Course semester is not current.");

        await _coursesRepository.UnsubscribeStudentFromCourseAsync(course, student);

        await _coursesRepository.SaveChangesAsync();

        return Unit.Value;
    }
}

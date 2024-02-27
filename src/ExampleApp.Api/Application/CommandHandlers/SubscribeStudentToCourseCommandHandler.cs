using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.SharedKernel.Contracts;
using ExampleApp.Api.Domain.Students.Contracts;
using ExampleApp.Api.Utils.Exceptions;
using ExampleApp.Api.Utils.Models;
using MediatR;

namespace ExampleApp.Api.Application.CommandHandlers;

internal class SubscribeStudentToCourseCommandHandler : IRequestHandler<SubscribeStudentToCourseCommand, Result<Unit>>
{
    private readonly ICoursesRepository _coursesRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<SubscribeStudentToCourseCommandHandler> _logger;

    public SubscribeStudentToCourseCommandHandler(
        ICoursesRepository coursesRepository,
        IStudentRepository studentRepository,
        ILogger<SubscribeStudentToCourseCommandHandler> logger)
    {
        _coursesRepository = coursesRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(SubscribeStudentToCourseCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Subscribing student to course.");

        var studentFullName = request.StudentCourse.Student.FullName;
        var courseId = request.StudentCourse.CourseId;

        _logger.LogInformation("Starting getting student and course.");

        var student = await _studentRepository.GetByNameAsync(studentFullName);

        var course = await _coursesRepository.GetCourseByIdAsync(courseId);

        if (student is null || course is null)
        {
            throw new EntityNotFoundException();
        }

        if (!course.IsCurrentOnSemester())
            throw new BusinessException("SemesterNotCurrent", "Course semester is not current.");

        await _coursesRepository.SubscribeStudentToCourseAsync(course, student);

        return Unit.Value;
    }
}

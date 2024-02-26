using ExampleApp.Api.Domain.Academia.Queries.Students;
using ExampleApp.Api.Domain.SharedKernel.Contracts;
using ExampleApp.Api.Utils.Models;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers.Students;

internal class GetStudentCoursesByCurrentSemesterHandler : IRequestHandler<GetStudentCoursesByCurrentSemesterQuery, Result<IReadOnlyList<StudentCourseView>>>
{
    private readonly IStudentCoursesRepository _studentCoursesRepository;

    public GetStudentCoursesByCurrentSemesterHandler(IStudentCoursesRepository studentCoursesRepository)
    {
        _studentCoursesRepository = studentCoursesRepository;
    }

    public async Task<Result<IReadOnlyList<StudentCourseView>>> Handle(GetStudentCoursesByCurrentSemesterQuery request, CancellationToken cancellationToken)
    {
        var studentCourses = await _studentCoursesRepository.GetAllByCurrentSemesterAsync();

        return studentCourses
            .Select(sc => new StudentCourseView(new StudentView(sc.Student.Id, sc.Student.FullName),
        sc.Courses
                .Select(c => c.Id)
                .ToList()))
            .ToList();
    }
}

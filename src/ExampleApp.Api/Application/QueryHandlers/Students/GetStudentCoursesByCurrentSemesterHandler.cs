using ExampleApp.Api.Domain.Academia.Queries.Students;
using ExampleApp.Api.Domain.SharedKernel.Contracts;
using ExampleApp.Api.Utils.Models;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers.Students;

internal class GetStudentCoursesByCurrentSemesterHandler : IRequestHandler<GetStudentCoursesByCurrentSemesterQuery, Result<IReadOnlyList<StudentCourseView>>>
{
    private readonly ICoursesRepository _coursesRepository;

    public GetStudentCoursesByCurrentSemesterHandler(ICoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }

    public async Task<Result<IReadOnlyList<StudentCourseView>>> Handle(GetStudentCoursesByCurrentSemesterQuery request, CancellationToken cancellationToken)
    {
        var studentCourses = await _coursesRepository.GetAllByCurrentSemesterAsync();

        return studentCourses
            .Select(sc => new StudentCourseView(new StudentView(sc.Student.Id, sc.Student.FullName),
        sc.Courses
                .Select(c => c.Id)
                .ToList()))
            .ToList();
    }
}

using ExampleApp.Api.Domain.Students;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.Commands;

internal record CreateStudentCourse(int studentId, string CourseId) : IRequest<Unit>
{
    internal StudentCourse ToStudentCourse()
    {
        return new StudentCourse
        {
            CourseId = CourseId,
            StudentId = studentId
        };
    }
}

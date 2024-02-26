using ExampleApp.Api.Domain.SharedKernel.Entities;

namespace ExampleApp.Api.Domain.SharedKernel.Contracts;

internal interface IStudentCoursesRepository
{
    Task<List<StudentCourses>> GetAllByCurrentSemesterAsync();
}

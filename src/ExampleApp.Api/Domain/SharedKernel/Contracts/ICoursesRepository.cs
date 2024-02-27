using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Domain.Students.Entities;

namespace ExampleApp.Api.Domain.SharedKernel.Contracts;

internal interface ICoursesRepository
{
    Task<List<StudentCourses>> GetAllByCurrentSemesterAsync();

    Task<Course> GetCourseByIdAsync(Guid courseId);

    Task UnsubscribeStudentFromCourseAsync(Course course, Student student);

    Task<StudentCourses> SubscribeStudentToCourseAsync(Course course, Student student);

    Task SaveChangesAsync();
}

using ExampleApp.Api.Domain.Students.Entities;

namespace ExampleApp.Api.Domain.Students.Contracts;

internal interface IStudentRepository
{
    Task<Student> GetByNameOrBadgeAsync(string fullName, int badge);

    Task SaveChangesAsync();
}

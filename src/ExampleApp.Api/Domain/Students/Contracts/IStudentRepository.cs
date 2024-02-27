using ExampleApp.Api.Domain.Students.Entities;

namespace ExampleApp.Api.Domain.Students.Contracts;

internal interface IStudentRepository
{
    Task<Student> GetByNameAsync(string fullName);
}

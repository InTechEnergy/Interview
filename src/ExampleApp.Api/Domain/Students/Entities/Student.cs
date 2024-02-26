using ExampleApp.Api.Domain.SharedKernel;

namespace ExampleApp.Api.Domain.Students.Entities;

internal class Student : ValueObject<Guid>
{
    public string FullName { get; set; }
}

using ExampleApp.Api.Domain.SharedKernel;

namespace ExampleApp.Api.Domain.Students.Entities;

internal class Student : ValueObject<Guid>
{
    public Student(string fullName)
    {
        FullName = fullName;
    }

    public string FullName { get; private set; }
}

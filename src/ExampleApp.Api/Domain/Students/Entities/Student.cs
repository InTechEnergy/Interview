using ExampleApp.Api.Domain.SharedKernel;

namespace ExampleApp.Api.Domain.Students.Entities;

internal class Student : ValueObject<Guid>
{
    public Student(string fullName)
    {
        FullName = fullName;
    }

    public Student(string fullName, int badge) : this(fullName)
    {
        badge = Badge;
    }

    public string FullName { get; private set; }

    public int Badge { get; private set; }
}

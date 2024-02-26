using ExampleApp.Api.Domain.Students.Entities;

namespace ExampleApp.Api.Domain.SharedKernel.Entities;

internal class StudentCourses : AggregateRoot<Guid>
{
    public Student Student { get; private set; }

    public virtual Guid StudentId { get; private set; }

    public ICollection<Course> Courses { get; private set; }

    public StudentCourses Add(Student student, Course course)
    {
        Courses ??= new List<Course>()
        {
            course
        };
        Student = student;

        return this;
    }
}

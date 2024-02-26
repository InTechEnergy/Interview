using ExampleApp.Api.Domain.Students.Entities;

namespace ExampleApp.Api.Domain.SharedKernel.Entities;

internal class StudentCourses : AggregateRoot<Guid>
{
    public StudentCourses(Student student, List<Course> courses)
    {
        Student = student;
        Courses = courses;
    }

    protected StudentCourses()
    {
    }

    public Student Student { get; private set; }

    public virtual Guid StudentId { get; private set; }

    public ICollection<Course> Courses { get; private set; }
}

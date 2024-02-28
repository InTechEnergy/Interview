using ExampleApp.Api.Domain.Academia;

namespace ExampleApp.Api.Domain.SharedKernel.Entities;

internal class Course : AggregateRoot<Guid>
{
    public Course(
        Guid id,
        string description,
        Semester semester,
        Lecturer lecturer)
    {
        Id = id;
        Description = description;
        Semester = semester;
        Lecturer = lecturer;
    }

    /// <summary>
    /// EF Constructor
    /// </summary>
    protected Course(Guid id, string description)
    {
        Id = id;
        Description = description;
    }
    public string Description { get; init; }
    public Semester Semester { get; protected init; }
    public Lecturer Lecturer { get; protected set; }

    public void UpdateProfessor(Lecturer newLecturer)
    {
        Lecturer = newLecturer ?? throw new ArgumentNullException(nameof(newLecturer));
    }

    public bool IsCurrentOnSemester()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        return Semester.Start <= date && Semester.End >= date;
    }
}

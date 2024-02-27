using ExampleApp.Api.Domain.Academia;

namespace ExampleApp.Api.Domain.SharedKernel.Entities;

internal class Course : AggregateRoot<Guid>
{
    public Course(
        Guid id,
        string description,
        Semester semester,
        Professor professor,
        DateTimeOffset createdOn,
        DateTimeOffset lastModifiedOn)
    {
        Id = id;
        Description = description;
        Semester = semester;
        Professor = professor;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }

    /// <summary>
    /// EF Constructor
    /// </summary>
    protected Course(Guid id, string description, DateTimeOffset createdOn, DateTimeOffset lastModifiedOn)
    {
        Id = id;
        Description = description;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }
    public string Description { get; init; }
    public Semester Semester { get; protected init; }
    public Professor Professor { get; protected set; }

    public void UpdateProfessor(Professor newProfessor)
    {
        Professor = newProfessor ?? throw new ArgumentNullException(nameof(newProfessor));
        LastModifiedOn = DateTimeOffset.UtcNow;
    }

    public bool IsCurrentOnSemester()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        return Semester.Start <= date && Semester.End >= date;
    }
}

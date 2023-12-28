using ExampleApp.Api.Domain.Students;

namespace ExampleApp.Api.Domain.Academia;

internal class Course : AggregateRoot<string>
{
    public Course(
        string id,
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
    protected Course(string id, string description,  DateTimeOffset createdOn, DateTimeOffset lastModifiedOn)
    {
        Id = id;
        Description = description;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }
    public string Description { get; init; }
    public Semester Semester { get; protected init; }
    public Professor Professor { get; protected set; }
    public virtual ICollection<StudentCourse> StudentCourses { get; set; }

    public void UpdateProfessor(Professor newProfessor)
    {
        Professor = newProfessor ?? throw new ArgumentNullException(nameof(newProfessor));
        LastModifiedOn = DateTimeOffset.UtcNow;
    }
}

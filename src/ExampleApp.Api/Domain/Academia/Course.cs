namespace ExampleApp.Api.Domain.Academia;

internal class Course : AggregateRoot<string>
{
    public Course(
        string id,
        string description,
        Semester semester,
        Professor lecturer,
        DateTimeOffset createdOn,
        DateTimeOffset lastModifiedOn)
    {
        Id = id;
        Description = description;
        Semester = semester;
        Lecturer = lecturer;
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
    public Professor Lecturer { get; protected set; }

    public void UpdateProfessor(Professor newLecturer)
    {
        Lecturer = newLecturer ?? throw new ArgumentNullException(nameof(newLecturer));
        LastModifiedOn = DateTimeOffset.UtcNow;
    }
}

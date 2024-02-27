using ExampleApp.Api.Domain.Students;
using System.ComponentModel.DataAnnotations.Schema;

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
        Lecturer = professor;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }

    /// <summary>
    /// EF Constructor
    /// </summary>
    protected Course(string id, string description, DateTimeOffset createdOn, DateTimeOffset lastModifiedOn)
    {
        Id = id;
        Description = description;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }
    public string Description { get; init; }

    [ForeignKey("ProfessorId")]
    public Professor Lecturer { get; protected set; }
    public Semester Semester { get; protected init; }
    public virtual ICollection<StudentCourse> StudentCourses { get; set; }

    public void UpdateProfessor(Professor newProfessor)
    {
        Lecturer = newProfessor ?? throw new ArgumentNullException(nameof(newProfessor));
        LastModifiedOn = DateTimeOffset.UtcNow;
    }
}

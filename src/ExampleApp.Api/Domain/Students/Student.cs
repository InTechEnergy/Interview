using ExampleApp.Api.Domain.Academia;
using System.ComponentModel.DataAnnotations;

namespace ExampleApp.Api.Domain.Students;

internal class Student : AggregateRoot<int>
{
    public Student(
        int id,
        string fullName,
        string badge,
        string residenceStatus,
        DateTimeOffset createdOn,
        DateTimeOffset lastModifiedOn)
    {
        Id = id;
        FullName = fullName;
        Badge = badge;
        ResidenceStatus = residenceStatus;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }
    [Required]
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Badge { get; set; }
    public string ResidenceStatus { get; set; }
    public virtual ICollection<StudentCourse> StudentCourses { get; set; }

}

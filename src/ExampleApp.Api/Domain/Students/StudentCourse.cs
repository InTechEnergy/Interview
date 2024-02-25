using ExampleApp.Api.Domain.Academia;

namespace ExampleApp.Api.Domain.Students;

internal class StudentCourse
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public required string CourseId { get; set; }

    public virtual Student? Student { get; set; }
    public virtual Course? Course { get; set; }
}

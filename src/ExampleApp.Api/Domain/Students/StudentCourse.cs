using ExampleApp.Api.Domain.Academia;

namespace ExampleApp.Api.Domain.Students;

internal class StudentCourse : Aggregate<int>
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string? CourseId { get; set; }

    public virtual required Student Student { get; set; }
    public virtual required Course Course { get; set; }
}

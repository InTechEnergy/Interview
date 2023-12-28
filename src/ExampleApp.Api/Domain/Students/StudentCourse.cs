using ExampleApp.Api.Domain.Academia;

namespace ExampleApp.Api.Domain.Students;

internal class StudentCourse
{
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public string CourseId { get; set; }
    public Course Course { get; set; }
}

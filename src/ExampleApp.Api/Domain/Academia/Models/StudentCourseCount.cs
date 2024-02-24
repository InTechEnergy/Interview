using ExampleApp.Api.Domain.Students;

namespace ExampleApp.Api.Domain.Academia.Models;

internal class StudentCourseCount
{
    public int CourseCount { get; set; }
    public required Student Student { get; set; }
}

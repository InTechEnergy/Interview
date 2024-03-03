using ExampleApp.Api.Domain.Students;

namespace ExampleApp.Api.Domain.Academia.Models;

internal class StudentCourseCountModel
{
    public int CourseCount { get; set; }
    public required Student Student { get; set; }
}

namespace ExampleApp.Api.Domain.Students;

internal class Student
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Badge { get; set; }
    public required ResidentStatus ResidentStatus { get; set; }

    public virtual required ICollection<StudentCourse> StudentCourses { get; set; }
}

namespace ExampleApp.Api.Domain.Students;


internal class Student : AggregateRoot<int>
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Badge { get; set; }
    public required ResidentStatus ResidentStatus { get; set; }

    public virtual ICollection<StudentCourse>? StudentCourses { get; set; }

}

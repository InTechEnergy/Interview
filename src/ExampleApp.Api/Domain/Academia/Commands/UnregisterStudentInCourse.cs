using MediatR;

namespace ExampleApp.Api.Domain.Academia.Commands;

public class UnregisterStudentInCourse : IRequest<bool>
{
    public string FullName { get; set; }
    public string Badge { get; set; }
    public string CourseId { get; set; }

}

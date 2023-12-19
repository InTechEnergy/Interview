using MediatR;

namespace ExampleApp.Api.Domain.Academia.Commands;

public class RegisterStudentInCourse : IRequest<bool>
{
    public string FullName { get; set; }
    public string Badge { get; set; }
    public string CourseId { get; set; }

}

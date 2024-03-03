using System.ComponentModel.DataAnnotations;

namespace ExampleApp.Api.Controllers.Models;

public class RegisterStudentToCourseModel
{
    public required string CourseId { get; set; }

    public string? FullName { get; set; }

    public string? BadgeNumber { get; set; }
}

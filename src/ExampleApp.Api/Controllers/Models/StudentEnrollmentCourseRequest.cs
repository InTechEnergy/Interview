using System.ComponentModel.DataAnnotations;

namespace ExampleApp.Api.Controllers.Models;

public class StudentEnrollmentCourseRequest
{
    public string? FullName { get; set; }
    public string? BadgeNumber { get; set; }

    [Required(ErrorMessage = "CourseId is required.")]
    [StringLength(100, ErrorMessage = "CourseId cannot be blank.", MinimumLength = 1)]
    public required string CourseId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if ((string.IsNullOrEmpty(FullName) && string.IsNullOrEmpty(BadgeNumber)) ||
            (!string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(BadgeNumber)))
        {
            yield return new ValidationResult(
                "Either FullName or BadgeNumber must be provided, but not both.",
                new[] { nameof(FullName), nameof(BadgeNumber) });
        }
    }
}

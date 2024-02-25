using ExampleApp.Api.Common;
using System.ComponentModel.DataAnnotations;

namespace ExampleApp.Api.Controllers.Models;

public class StudentEnrollmentCourseRequestModel : IValidatableObject
{
    public string? FullName { get; set; }
    public string? BadgeNumber { get; set; }

    [Required(ErrorMessage = Constants.COURSE_ID_REQUIRED)]
    public string? CourseId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(FullName) && string.IsNullOrWhiteSpace(BadgeNumber))
        {
            yield return new ValidationResult(
                Constants.FULLNAME_OR_BADGENUMBER_REQUIRED,
                new[] { nameof(FullName) });
        }
    }
}

namespace ExampleApp.Api.Controllers.Models;

public record SemesterCoursesResponseModel : KeyNameModel
{
    public SemesterCoursesResponseModel(string key, string name) : base(key, name)
    {
    }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public required List<CourseModel> Courses { get; set; }
}

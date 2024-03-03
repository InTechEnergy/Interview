namespace ExampleApp.Api.Services.Interfaces;

public interface ICourseService
{
    public Task<bool> IsCourseCurrentAsync(string courseId);

    public Task<bool> IsCoursePastAsync(string courseId);
}

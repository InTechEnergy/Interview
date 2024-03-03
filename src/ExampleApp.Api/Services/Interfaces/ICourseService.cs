namespace ExampleApp.Api.Services.Interfaces;

public interface ICourseService
{
    public Task<bool> IsCourseCurrent(string courseId);
}

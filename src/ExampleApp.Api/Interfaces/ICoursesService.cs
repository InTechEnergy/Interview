namespace ExampleApp.Api.Interfaces;

public interface ICoursesService
{
    public Task<bool> IsCourseActive(string courseId);
}

using ExampleApp.Api.Controllers;

namespace ExampleApp.Api.CrossCutting.DI;

public static class ServiceEndpointsExtensions
{
    public static void MapEndpoints(this WebApplication webApplication)
    {
        StudentCoursesEndpoints.MapEndpoints(webApplication);
        CoursesEndpoint.MapEndpoints(webApplication);
    }
}

using ExampleApp.Api.Controllers;

namespace ExampleApp.Api.Interfaces;

public interface IFileProcessorService
{
    public bool CanProcess(string contentType, string extension);
    public List<StudentEnrollmentCourseBulkRequestModel> Process(IFormFile file);
}

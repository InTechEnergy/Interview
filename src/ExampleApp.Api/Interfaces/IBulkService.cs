using ExampleApp.Api.Controllers.Models;

namespace ExampleApp.Api.Interfaces;

public interface IBulkService
{
    public Task BulkInsertStudents(List<StudentEnrollmentCourseBulkRequestModel> students);
}

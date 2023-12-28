using ExampleApp.Api.Domain.Academia;

namespace ExampleApp.Api.Controllers.Models;

public record StudentModel(int Id, string FullName, int CourseCount, string Badge, string ResidenceStatus);

using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Students;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record FindStudentQuery(StudentEnrollmentCourseRequestModel request) : IRequest<Student?>;

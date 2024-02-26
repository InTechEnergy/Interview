using System.Linq.Expressions;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Utils;

namespace ExampleApp.Api.Domain.SharedKernel.Specifications;

internal class CurrentCoursesSpecification : ISpecification<StudentCourses>
{
    public Expression<Func<StudentCourses, bool>> Criteria { get; }

    public CurrentCoursesSpecification()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        Criteria = sc => sc.Courses.Any(c => c.Semester.Start <= date && c.Semester.End >= date);
    }
}

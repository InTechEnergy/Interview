using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Domain.SharedKernel.Specifications;
using ExampleApp.Api.Domain.Students.Entities;
using ExampleApp.Api.Utils;
using FluentAssertions;

namespace ExampleApp.Tests.Specifications;

public class CurrentCoursesSpecificationTests
{
    [Fact]
    public void CurrentCoursesSpecification_WhenStudentRegisteredDifferentCourse_ShouldBringCurrentOnes()
    {
        var studentCourses = new StudentCourses(
            new Student("John Snow"),
            new List<Course>()
            {
                new Course(
                    id: Guid.NewGuid(),
                    description: "Math",
                    semester: new Semester()
                    {
                        Description = "This is the semester",
                        Start = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20)),
                        End = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(20))
                    },
                    professor: new Professor()
                    {
                        FullName = "John Snow"
                    }
                ),
                new Course(
                    id: Guid.NewGuid(),
                    description: "Philosophy",
                    semester: new Semester()
                    {
                        Description = "This is the semester2",
                        Start = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)),
                        End = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(20))
                    },
                    professor: new Professor()
                    {
                        FullName = "Dumbledore"
                    }
                ),
                new Course(
                    id: Guid.NewGuid(),
                    description: "History",
                    semester: new Semester()
                    {
                        Description = "This is the semester",
                        Start = DateOnly.FromDateTime(DateTime.UtcNow),
                        End = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5))
                    },
                    professor: new Professor()
                    {
                        FullName = "John Snow"
                    }
                ),
            });

        var currentCoursesSpecification = new CurrentCoursesSpecification();

        var evaluator = new SpecificationEvaluator();

        var studentCoursesQuery = new List<StudentCourses>() { studentCourses };

        var result = evaluator.GetQuery(studentCoursesQuery.AsQueryable(), currentCoursesSpecification).ToList();

        result
            .Should()
            .ContainSingle();

        var coursesResult = result.First().Courses;

        coursesResult
            .Should()
            .ContainSingle(c => c.Description == "Math");
    }
}

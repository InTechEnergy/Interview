using System.Collections.Immutable;
using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia.QueryHandlers;

internal class GetAllStudentsHandler : IRequestHandler<GetAllStudentsQuery, ICollection<StudentModel>>
{
    private readonly AcademiaDbContext _context;

    public GetAllStudentsHandler(AcademiaDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<StudentModel>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        var students = await _context.Students
            .Select(s => new StudentModel(s.Id, s.FullName, s.StudentCourses.Count(sc => sc.Course.Semester.Start <= request.ActiveOn && request.ActiveOn <= sc.Course.Semester.End), s.Badge, s.ResidenceStatus) { })
            .ToListAsync();

        return students;
    }
    
}

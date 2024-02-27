using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Students.Contracts;
using ExampleApp.Api.Domain.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Infrastructure.Repositories;

internal class StudentRepository : IStudentRepository
{
    private readonly AcademiaDbContext _dbContext;

    public StudentRepository(AcademiaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public  async Task<Student> GetByNameAsync(string fullName) => await _dbContext.Students
        .SingleOrDefaultAsync(s => s.FullName == fullName);
}

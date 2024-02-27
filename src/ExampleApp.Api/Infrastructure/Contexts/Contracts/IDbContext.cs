namespace ExampleApp.Api.Domain.Academia.Contracts;

public interface IDbContext
{
    Task<int> SaveChangesAsync();
    int SaveChanges();
}

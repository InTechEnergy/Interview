using ExampleApp.Api.Controllers;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.SharedKernel.Contracts;
using ExampleApp.Api.Domain.Students.Contracts;
using ExampleApp.Api.Infrastructure.Repositories;
using ExampleApp.Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.CrossCutting.DI;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureBaseServices();
        services.ConfigureServices();
        services.ConfigureDatabases(configuration);

        return services;
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ICoursesRepository, CoursesRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ISpecificationEvaluator, SpecificationEvaluator>();
    }

    private static void ConfigureBaseServices(
        this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

    }

    private static void ConfigureDatabases(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AcademiaDbContext>(
            opt => opt.UseSqlServer(configuration.GetConnectionString("Default")));
    }
}

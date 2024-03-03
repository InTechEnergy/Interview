using ExampleApp.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleApp.Tests;

public class DatabaseFixture
{
    public DatabaseFixture()
    {
        Configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection()
            .AddHttpClient();
        services.AddDbContext<AcademiaDbContext>(
            opt => opt.UseSqlServer(Configuration.GetConnectionString("Default")));
        services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssemblyContaining<AcademiaDbContext>());

        Services = services.BuildServiceProvider();
    }

    public ServiceProvider Services { get; }

    public IConfigurationRoot Configuration { get; }
}

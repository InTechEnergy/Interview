using ExampleApp.Api.CrossCutting.DI;
using ExampleApp.Api.Domain.Academia;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace ExampleApp.Tests;

[CollectionDefinition(nameof(TestApplicationCollection))]
public class TestApplicationCollection : ICollectionFixture<TestApplication>
{
}

public class TestApplication : IAsyncLifetime
{
    public ServiceProvider Services { get; protected set; }
    public IConfigurationRoot Configuration { get; protected set; }
    public IMediator Mediator { get; protected set; }
    internal AcademiaDbContext DbContext { get; set; }

    private SqlConnection _dbConnection;
    private Respawner _respawner = default!;

    public async Task InitializeAsync()
    {
        Configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection()
            .AddHttpClient();


        services.AddServices(Configuration);
        Services = services.BuildServiceProvider();

        DbContext = Services.GetService<AcademiaDbContext>();
        Mediator = Services.GetService<IMediator>();

        _dbConnection = new SqlConnection(Configuration.GetConnectionString("Default"));

        await SetupDatabase();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    private async Task SetupDatabase()
    {
        await DbContext.Database.MigrateAsync();
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions {
            DbAdapter = DbAdapter.SqlServer,
            TablesToInclude = new Respawn.Graph.Table[] {
                "Semesters",
                "Courses",
                "Students",
                "StudentCourses",
                "Professors"

            },
            WithReseed = true
        });
        await ResetDatabase();
    }

    public async Task ResetDatabase()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
}

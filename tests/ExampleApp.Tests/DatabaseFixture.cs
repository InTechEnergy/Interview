using ExampleApp.Api.CrossCutting.DI;
using ExampleApp.Api.Domain.Academia;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace ExampleApp.Tests;


[CollectionDefinition(nameof(TestApplicationCollection))]
public class TestApplicationCollection : ICollectionFixture<DatabaseFixture>
{
}

public class DatabaseFixture : IAsyncLifetime
{
    private AcademiaDbContext? _db;

    private Respawner _respawner = default!;

    private SqlConnection _dbConnection = default!;

    public ServiceProvider Services { get; protected set; }

    public IConfigurationRoot Configuration { get; protected set; }

    internal AcademiaDbContext Db => _db;

    internal IMediator Mediator;

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

        _db = Services.GetService<AcademiaDbContext>();
        Mediator = Services.GetService<IMediator>();

        _dbConnection = new SqlConnection(Configuration.GetConnectionString("Default"));

        await SetupDatabase();
    }

    private async Task SetupDatabase()
    {
        await _db.Database.MigrateAsync();
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

    public Task DisposeAsync()
    {
        _dbConnection.Dispose();
        _db?.Dispose();
        Services.Dispose();

        _respawner.ResetAsync(_dbConnection);

        return default;
    }
}

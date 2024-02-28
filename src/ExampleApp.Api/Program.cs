using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Interfaces;
using ExampleApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
// Add services to the container.

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBulkService, BulkService>();
builder.Services.AddScoped<ICoursesService, CoursesService>();
builder.Services.AddScoped<IValidationsService, ValidationsService>();
builder.Services.AddDbContext<AcademiaDbContext>(
    opt => opt.UseSqlServer(
        config.GetConnectionString("Default"),
        sqlServerOptionsAction: sqlOptions =>
        {
            _ = sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }));
builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddTransient<IFileProcessorService, CsvFileProcessorService>();
builder.Services.AddTransient<IFileProcessorService, ExcelFileProcessorService>();


var app = builder.Build();

// Ensure the database is created.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AcademiaDbContext>();
        _ = context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


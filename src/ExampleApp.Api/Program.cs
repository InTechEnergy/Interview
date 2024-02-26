using ExampleApp.Api.CrossCutting.DI;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Domain.Students.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddServices(config);

var app = builder.Build();

app.MapEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

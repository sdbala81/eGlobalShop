using System.Reflection;

using ElementLogiq.eGlobalShop.Orders.Application;
using ElementLogiq.eGlobalShop.Orders.Infrastructure;
using ElementLogiq.eGlobalShop.Orders.WebApi;
using ElementLogiq.eGlobalShop.Orders.WebApi.Extensions;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddSwaggerGenWithAuth();

builder.Services.AddApplicationServices()
    .AddWebApiServices()
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();
    app.ApplyMigrations();
}

app.MapHealthChecks(
    "health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.MapHealthChecks(
    "ready",
    new HealthCheckOptions
    {
        Predicate = healthCheck => healthCheck.Tags.Contains("ready")
    });

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

await app.RunAsync()
    .ConfigureAwait(false);

static Task UpdateOrder<T>(T jsonData)
{
    Console.WriteLine($"Order update: {jsonData}");

    return Task.CompletedTask;
}

static Task CreateNewOrder<T>(T jsonData)
{
    Console.WriteLine($"Create new order: {jsonData}");

    return Task.CompletedTask;
}

static Task Send(string jsonData)
{
    Console.WriteLine($"Delete order: {jsonData}");

    return Task.CompletedTask;
}

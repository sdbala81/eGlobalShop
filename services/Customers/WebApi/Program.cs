using System.Reflection;

using ElementLogiq.eGlobalShop.Customers.Application;
using ElementLogiq.eGlobalShop.Customers.Infrastructure;
using ElementLogiq.eGlobalShop.Customers.WebApi;
using ElementLogiq.eGlobalShop.Customers.WebApi.Extensions;
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

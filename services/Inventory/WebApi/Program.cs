using System.Reflection;

using ElementLogiq.eGlobalShop.Inventory.Application;
using ElementLogiq.eGlobalShop.Inventory.Infrastructure;
using ElementLogiq.eGlobalShop.Inventory.WebApi;
using ElementLogiq.eGlobalShop.Inventory.WebApi.Extensions;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddSwaggerGenWithAuth();

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices()
    .AddWebApiServices();

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

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

await app.RunAsync()
    .ConfigureAwait(false);

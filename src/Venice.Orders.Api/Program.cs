using Serilog;
using Venice.Orders.Api.Extensions;
using Venice.Orders.Application.Extensions;
using Venice.Orders.Infrastructure.Extensions;

SerilogExtensions.ConfigureSerilog();

try
{
    Log.Information("Starting Venice Orders API application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddApiControllers();
    builder.Services.AddSwaggerDocumentation();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddJwtAuthentication(builder.Configuration, builder.Environment);
    builder.Services.AddApiRateLimiting();
    builder.Services.AddApiHealthChecks(builder.Configuration);

    var app = builder.Build();

    await app.MigrateDatabaseAsync();
    app.ConfigureMiddlewarePipeline();

    Log.Information("Application started successfully");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }

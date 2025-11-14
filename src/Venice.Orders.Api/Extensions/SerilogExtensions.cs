using Serilog;
using Serilog.Events;

namespace Venice.Orders.Api.Extensions;

public static class SerilogExtensions
{
    public static void ConfigureSerilog()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var isDevelopment = environment == "Development";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .WriteTo.Console(outputTemplate: isDevelopment 
                ? "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                : "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File("logs/venice-orders-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        if (isDevelopment)
        {
            Console.Title = "Venice Orders API - Development";
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
    }
}


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Venice.Orders.Infrastructure.Data.SqlServer;

namespace Venice.Orders.IntegrationTests.Helpers;

public class IntegrationTestBase : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:SqlServer", "Server=localhost,1433;Database=VeniceOrders_Test;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;" },
                { "ConnectionStrings:MongoDb", "mongodb://localhost:27017" },
                { "ConnectionStrings:Redis", "localhost:6379" },
                { "ConnectionStrings:RabbitMQ", "amqp://guest:guest@localhost:5672" },
                { "MongoDb:DatabaseName", "VeniceOrders_Test" },
                { "Jwt:SecretKey", "TestSecretKey_ForIntegrationTests_Minimum32Characters" },
                { "Jwt:Issuer", "VeniceOrders" },
                { "Jwt:Audience", "VeniceOrders" },
                { "Jwt:ExpirationMinutes", "60" }
            });
        });

        builder.ConfigureServices(services =>
        {
            var authDescriptors = services.Where(
                d => d.ServiceType?.FullName?.Contains("Authentication") == true ||
                     d.ImplementationType?.FullName?.Contains("JwtBearer") == true)
                .ToList();
            
            foreach (var descriptor in authDescriptors)
            {
                services.Remove(descriptor);
            }

            var healthCheckDescriptors = services.Where(
                d => d.ServiceType?.FullName?.Contains("HealthCheck") == true ||
                     d.ImplementationType?.FullName?.Contains("HealthCheck") == true ||
                     (d.ServiceType == typeof(Microsoft.Extensions.Hosting.IHostedService) &&
                      d.ImplementationType?.FullName?.Contains("HealthCheck") == true))
                .ToList();
            
            foreach (var descriptor in healthCheckDescriptors)
            {
                services.Remove(descriptor);
            }

            services.AddHealthChecks();
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", null);
            
            services.PostConfigure<Microsoft.AspNetCore.Authentication.AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
                options.DefaultScheme = "Test";
            });
        });

        builder.UseEnvironment("Test");
    }
}


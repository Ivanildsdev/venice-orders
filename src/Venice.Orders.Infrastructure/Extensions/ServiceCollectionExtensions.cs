using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StackExchange.Redis;
using Venice.Orders.Domain.Interfaces;
using Venice.Orders.Domain.Interfaces.Repositories;
using Venice.Orders.Infrastructure.Data.MongoDb;
using Venice.Orders.Infrastructure.Data.SqlServer;
using Venice.Orders.Infrastructure.Data.SqlServer.Repositories;
using Venice.Orders.Infrastructure.Data.MongoDb.Repositories;
using Venice.Orders.Infrastructure.Cache;
using Venice.Orders.Infrastructure.Messaging;
using Venice.Orders.Application.Interfaces;

namespace Venice.Orders.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddRealInfrastructure(services, configuration);
        return services;
    }

    private static void AddRealInfrastructure(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<OrdersDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("SqlServer"),
                sqlOptions => sqlOptions.MigrationsAssembly("Venice.Orders.Infrastructure")));

        services.AddSingleton<IMongoClient>(sp =>
        {
            var connectionString = configuration.GetConnectionString("MongoDb");
            return new MongoClient(connectionString);
        });

        services.AddSingleton<MongoDbContext>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var databaseName = configuration["MongoDb:DatabaseName"] ?? "VeniceOrders";
            var context = new MongoDbContext(mongoClient, databaseName);
            
            try
            {
                Task.Run(async () => await context.CreateIndexesAsync()).Wait();
            }
            catch
            {
                // Ignore index creation errors - indexes will be created on first use
            }
            
            return context;
        });

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IUnitOfWork, Venice.Orders.Infrastructure.UnitOfWork.UnitOfWork>();

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var connectionString = configuration.GetConnectionString("Redis") 
                ?? throw new InvalidOperationException("Redis connection string not configured");
            return ConnectionMultiplexer.Connect(connectionString);
        });
        services.AddScoped<ICacheService, RedisCacheService>();

        services.AddSingleton<IMessageBus>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            return new RabbitMqService(config);
        });
    }
}

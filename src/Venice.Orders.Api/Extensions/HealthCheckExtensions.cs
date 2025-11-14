namespace Venice.Orders.Api.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddApiHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlServerConnection = configuration.GetConnectionString("SqlServer") 
            ?? throw new InvalidOperationException("SQL Server connection string not configured");
        var mongoDbConnection = configuration.GetConnectionString("MongoDb") 
            ?? throw new InvalidOperationException("MongoDB connection string not configured");
        var redisConnection = configuration.GetConnectionString("Redis") 
            ?? throw new InvalidOperationException("Redis connection string not configured");
        var rabbitMqConnection = configuration.GetConnectionString("RabbitMQ") 
            ?? throw new InvalidOperationException("RabbitMQ connection string not configured");

        services.AddHealthChecks()
            .AddSqlServer(sqlServerConnection)
            .AddMongoDb(mongoDbConnection)
            .AddRedis(redisConnection)
            .AddRabbitMQ(rabbitConnectionString: rabbitMqConnection);

        return services;
    }
}


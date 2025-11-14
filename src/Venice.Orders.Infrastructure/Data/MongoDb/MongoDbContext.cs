using MongoDB.Driver;
using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Infrastructure.Data.MongoDb;

/// <summary>
/// Contexto MongoDB - será implementado completamente na Fase 4
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoClient client, string databaseName)
    {
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<OrderItem> OrderItems => 
        _database.GetCollection<OrderItem>("OrderItems");

    public async Task CreateIndexesAsync()
    {
        var indexBuilder = Builders<OrderItem>.IndexKeys;
        var indexModel = new CreateIndexModel<OrderItem>(
            indexBuilder.Ascending(i => i.OrderId)
        );
        
        await OrderItems.Indexes.CreateOneAsync(indexModel);
    }
}

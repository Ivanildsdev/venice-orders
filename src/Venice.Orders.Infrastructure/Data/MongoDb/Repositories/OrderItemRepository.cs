using MongoDB.Driver;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Interfaces.Repositories;
using Venice.Orders.Infrastructure.Data.MongoDb;

namespace Venice.Orders.Infrastructure.Data.MongoDb.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly MongoDbContext _context;

    public OrderItemRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(
        Guid orderId, 
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<OrderItem>.Filter.Eq(i => i.OrderId, orderId);
        
        var cursor = await _context.OrderItems
            .FindAsync(filter, cancellationToken: cancellationToken);
        
        return await cursor.ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(
        IEnumerable<OrderItem> items, 
        CancellationToken cancellationToken = default)
    {
        await _context.OrderItems
            .InsertManyAsync(items, cancellationToken: cancellationToken);
    }
}


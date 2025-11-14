using Microsoft.EntityFrameworkCore;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Interfaces.Repositories;
using Venice.Orders.Infrastructure.Data.SqlServer;

namespace Venice.Orders.Infrastructure.Data.SqlServer.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _context;

    public OrderRepository(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Order> CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        return order;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .AnyAsync(p => p.Id == id, cancellationToken);
    }
}


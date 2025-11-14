using Microsoft.EntityFrameworkCore;
using Venice.Orders.Domain.Interfaces;
using Venice.Orders.Domain.Interfaces.Repositories;
using Venice.Orders.Infrastructure.Data.MongoDb;
using Venice.Orders.Infrastructure.Data.MongoDb.Repositories;
using Venice.Orders.Infrastructure.Data.SqlServer;
using Venice.Orders.Infrastructure.Data.SqlServer.Repositories;

namespace Venice.Orders.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly OrdersDbContext _sqlContext;
    private readonly MongoDbContext _mongoContext;
    private IOrderRepository? _orderRepository;
    private IOrderItemRepository? _orderItemRepository;

    public UnitOfWork(
        OrdersDbContext sqlContext,
        MongoDbContext mongoContext)
    {
        _sqlContext = sqlContext;
        _mongoContext = mongoContext;
    }

    public IOrderRepository Orders =>
        _orderRepository ??= new OrderRepository(_sqlContext);

    public IOrderItemRepository Items =>
        _orderItemRepository ??= new OrderItemRepository(_mongoContext);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _sqlContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _sqlContext.SaveChangesAsync(cancellationToken);
            
            return true;
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        var entries = _sqlContext.ChangeTracker.Entries()
            .Where(e => e.State != EntityState.Unchanged)
            .ToList();

        foreach (var entry in entries)
        {
            entry.State = EntityState.Detached;
        }

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        _sqlContext?.Dispose();
    }
}

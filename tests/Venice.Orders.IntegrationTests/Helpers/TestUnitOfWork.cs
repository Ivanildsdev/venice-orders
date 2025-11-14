using Microsoft.EntityFrameworkCore;
using Venice.Orders.Domain.Interfaces;
using Venice.Orders.Domain.Interfaces.Repositories;
using Venice.Orders.Infrastructure.Data.MongoDb;
using Venice.Orders.Infrastructure.Data.SqlServer;
using Venice.Orders.Infrastructure.Data.SqlServer.Repositories;

namespace Venice.Orders.IntegrationTests.Helpers;

public class TestUnitOfWork : IUnitOfWork
{
    private readonly OrdersDbContext _sqlContext;
    private readonly MongoDbContext _mongoContext;
    private readonly IOrderItemRepository _orderItemRepository;
    private IOrderRepository? _orderRepository;

    public TestUnitOfWork(
        OrdersDbContext sqlContext,
        MongoDbContext mongoContext,
        IOrderItemRepository orderItemRepository)
    {
        _sqlContext = sqlContext;
        _mongoContext = mongoContext;
        _orderItemRepository = orderItemRepository;
    }

    public IOrderRepository Orders =>
        _orderRepository ??= new OrderRepository(_sqlContext);

    public IOrderItemRepository Items => _orderItemRepository;

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


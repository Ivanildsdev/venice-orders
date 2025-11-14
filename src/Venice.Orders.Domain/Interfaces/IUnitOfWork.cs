using Venice.Orders.Domain.Interfaces.Repositories;

namespace Venice.Orders.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }

    IOrderItemRepository Items { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<bool> CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);
}

using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Domain.Interfaces.Repositories;

public interface IOrderItemRepository
{
    Task<IEnumerable<OrderItem>> GetByOrderIdAsync(
        Guid orderId, 
        CancellationToken cancellationToken = default);

    Task CreateAsync(
        IEnumerable<OrderItem> items, 
        CancellationToken cancellationToken = default);
}


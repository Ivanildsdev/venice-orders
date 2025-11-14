using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para repositório de itens de pedido
/// </summary>
public interface IOrderItemRepository
{
    /// <summary>
    /// Obtém todos os itens de um pedido
    /// </summary>
    /// <param name="orderId">ID do pedido</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de itens do pedido</returns>
    Task<IEnumerable<OrderItem>> GetByOrderIdAsync(
        Guid orderId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria múltiplos itens de pedido
    /// </summary>
    /// <param name="items">Lista de itens a serem criados</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    Task CreateAsync(
        IEnumerable<OrderItem> items, 
        CancellationToken cancellationToken = default);
}


using Venice.Orders.Domain.Interfaces.Repositories;

namespace Venice.Orders.Domain.Interfaces;

/// <summary>
/// Interface para Unit of Work - coordena transações entre múltiplos repositórios
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Repositório de pedidos
    /// </summary>
    IOrderRepository Orders { get; }

    /// <summary>
    /// Repositório de itens de pedido
    /// </summary>
    IOrderItemRepository Items { get; }

    /// <summary>
    /// Salva as alterações pendentes no contexto
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Número de entidades afetadas</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirma todas as transações pendentes
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se commitado com sucesso</returns>
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Reverte todas as transações pendentes
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}

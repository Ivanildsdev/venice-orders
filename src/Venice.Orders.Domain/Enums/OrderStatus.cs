namespace Venice.Orders.Domain.Enums;

/// <summary>
/// Representa os possíveis status de um pedido
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Pedido criado e aguardando confirmação
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Pedido confirmado e em processamento
    /// </summary>
    Confirmed = 2,

    /// <summary>
    /// Pedido cancelado
    /// </summary>
    Cancelled = 3,

    /// <summary>
    /// Pedido finalizado
    /// </summary>
    Completed = 4
}


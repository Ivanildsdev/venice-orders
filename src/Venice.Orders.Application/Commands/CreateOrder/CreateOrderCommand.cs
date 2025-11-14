using MediatR;
using Venice.Orders.Application.DTOs;

namespace Venice.Orders.Application.Commands.CreateOrder;

/// <summary>
/// Command para criar um novo pedido
/// </summary>
public record CreateOrderCommand(
    Guid CustomerId,
    DateTime Date,
    List<OrderItemRequest> Items
) : IRequest<OrderDto>;

/// <summary>
/// Request para item de pedido
/// </summary>
public record OrderItemRequest(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice
);


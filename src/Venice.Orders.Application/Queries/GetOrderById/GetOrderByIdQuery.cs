using MediatR;
using Venice.Orders.Application.DTOs;

namespace Venice.Orders.Application.Queries.GetOrderById;

/// <summary>
/// Query para obter um pedido por ID
/// </summary>
public record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto?>;


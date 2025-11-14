using Venice.Orders.Domain.Enums;

namespace Venice.Orders.Application.DTOs;

/// <summary>
/// DTO para representar um pedido
/// </summary>
public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime Date { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Total { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}


using System.ComponentModel.DataAnnotations;

namespace Venice.Orders.Api.Models;

/// <summary>
/// Request DTO para criar um pedido
/// </summary>
public class CreateOrderRequest
{
    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [MinLength(1)]
    public List<OrderItemRequestDto> Items { get; set; } = new();
}

/// <summary>
/// Request DTO para item de pedido
/// </summary>
public class OrderItemRequestDto
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }
}


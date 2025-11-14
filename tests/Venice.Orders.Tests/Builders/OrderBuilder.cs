using Venice.Orders.Application.Commands.CreateOrder;
using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Tests.Builders;

public class OrderBuilder
{
    private Guid _customerId = Guid.NewGuid();
    private DateTime _date = DateTime.UtcNow;
    private readonly List<OrderItemRequest> _items = new();

    public OrderBuilder WithCustomerId(Guid customerId)
    {
        _customerId = customerId;
        return this;
    }

    public OrderBuilder WithDate(DateTime date)
    {
        _date = date;
        return this;
    }

    public OrderBuilder WithItem(Guid productId, int quantity, decimal unitPrice)
    {
        _items.Add(new OrderItemRequest(productId, quantity, unitPrice));
        return this;
    }

    public CreateOrderCommand BuildCommand()
    {
        return new CreateOrderCommand(_customerId, _date, _items);
    }

    public Order BuildDomain()
    {
        var order = new Order(_customerId, _date);
        foreach (var item in _items)
        {
            order.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
        }
        return order;
    }
}


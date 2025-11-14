using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Enums;
using Venice.Orders.Domain.Exceptions;

namespace Venice.Orders.Tests.Unit.Domain;

public class OrderTests
{
    [Fact]
    public void CreateOrder_WithValidData_ShouldCreateOrderWithPendingStatus()
    {
        var customerId = Guid.NewGuid();
        var date = DateTime.UtcNow;

        var order = new Order(customerId, date);

        Assert.NotNull(order);
        Assert.Equal(customerId, order.CustomerId);
        Assert.Equal(date, order.Date);
        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Equal(0, order.Total);
        Assert.Empty(order.Items);
    }

    [Fact]
    public void CreateOrder_WithFutureDate_ShouldThrowException()
    {
        var customerId = Guid.NewGuid();
        var futureDate = DateTime.UtcNow.AddDays(1);

        Assert.Throws<DomainException>(() => new Order(customerId, futureDate));
    }

    [Fact]
    public void AddItem_WithValidData_ShouldAddItemAndRecalculateTotal()
    {
        var order = new Order(Guid.NewGuid(), DateTime.UtcNow);
        var productId = Guid.NewGuid();
        var quantity = 2;
        var unitPrice = 10.50m;

        order.AddItem(productId, quantity, unitPrice);

        Assert.Single(order.Items);
        Assert.Equal(21.00m, order.Total);
        var item = order.Items.First();
        Assert.Equal(productId, item.ProductId);
        Assert.Equal(quantity, item.Quantity);
        Assert.Equal(unitPrice, item.UnitPrice);
    }
}


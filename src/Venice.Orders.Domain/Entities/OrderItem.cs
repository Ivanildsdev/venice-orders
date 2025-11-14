using Venice.Orders.Domain.Exceptions;

namespace Venice.Orders.Domain.Entities;

public class OrderItem : Entity
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Subtotal => Quantity * UnitPrice;

    private OrderItem() : base() { }

    public OrderItem(Guid orderId, Guid productId, int quantity, decimal unitPrice) : base()
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = ValidateQuantity(quantity);
        UnitPrice = ValidatePrice(unitPrice);
    }

    public void AddQuantity(int additionalQuantity)
    {
        if (additionalQuantity <= 0)
            throw new DomainException("Additional quantity must be greater than zero");

        Quantity = ValidateQuantity(Quantity + additionalQuantity);
        MarkAsUpdated();
    }

    public bool IsSameProductAndPrice(Guid productId, decimal unitPrice)
    {
        return ProductId == productId && UnitPrice == unitPrice;
    }

    private static int ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero");
        
        return quantity;
    }

    private static decimal ValidatePrice(decimal price)
    {
        if (price <= 0)
            throw new DomainException("Unit price must be greater than zero");
        
        return price;
    }
}


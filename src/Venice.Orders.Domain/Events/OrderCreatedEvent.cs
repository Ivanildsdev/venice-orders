namespace Venice.Orders.Domain.Events;

public class OrderCreatedEvent
{
    public Guid OrderId { get; }
    public Guid CustomerId { get; }
    public DateTime Date { get; }
    public decimal Total { get; }

    public OrderCreatedEvent(Guid orderId, Guid customerId, DateTime date, decimal total)
    {
        OrderId = orderId;
        CustomerId = customerId;
        Date = date;
        Total = total;
    }
}


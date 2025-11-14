using MediatR;

namespace Venice.Orders.Application.Notifications.OrderCreated;

public class OrderCreatedNotification : INotification
{
    public Guid OrderId { get; }
    public Guid CustomerId { get; }
    public DateTime Date { get; }
    public decimal Total { get; }

    public OrderCreatedNotification(Guid orderId, Guid customerId, DateTime date, decimal total)
    {
        OrderId = orderId;
        CustomerId = customerId;
        Date = date;
        Total = total;
    }
}


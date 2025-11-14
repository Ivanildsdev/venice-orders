using Venice.Orders.Domain.Enums;
using Venice.Orders.Domain.Exceptions;

namespace Venice.Orders.Domain.Entities;

public class Order : Entity
{
    #region Properties
    public Guid CustomerId { get; private set; }
    public DateTime Date { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal Total { get; private set; }
    
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    #endregion
    

    #region Constructors
    private Order() : base() { }

    public Order(Guid customerId, DateTime date) : base()
    {
        CustomerId = customerId;
        Date = ValidateDate(date);
        Status = OrderStatus.Pending;
        Total = 0;
    }

    #endregion

    #region Methods
    public void AddItem(Guid productId, int quantity, decimal unitPrice)
    {
        var existingItem = _items.FirstOrDefault(i => i.IsSameProductAndPrice(productId, unitPrice));

        if (existingItem != null)
        {
            existingItem.AddQuantity(quantity);
        }
        else
        {
            var newItem = new OrderItem(Id, productId, quantity, unitPrice);
            _items.Add(newItem);
        }

        RecalculateTotal();
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        if (!CanChangeStatus(newStatus))
            throw new DomainException($"Cannot change status from {Status} to {newStatus}");

        Status = newStatus;
        MarkAsUpdated();
    }

    private void RecalculateTotal()
    {
        Total = _items.Sum(i => i.Subtotal);
        MarkAsUpdated();
    }

    private static DateTime ValidateDate(DateTime date)
    {
        if (date > DateTime.UtcNow)
            throw new DomainException("Order date cannot be in the future");
        
        return date;
    }

    private bool CanChangeStatus(OrderStatus newStatus)
    {
        return Status switch
        {
            OrderStatus.Pending => newStatus == OrderStatus.Confirmed || newStatus == OrderStatus.Cancelled,
            OrderStatus.Confirmed => newStatus == OrderStatus.Completed || newStatus == OrderStatus.Cancelled,
            OrderStatus.Cancelled => false,
            OrderStatus.Completed => false,
            _ => false
        };
    }
    #endregion
}


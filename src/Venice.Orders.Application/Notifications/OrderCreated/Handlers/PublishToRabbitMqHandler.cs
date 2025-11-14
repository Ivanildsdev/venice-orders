using MediatR;
using Microsoft.Extensions.Logging;
using Venice.Orders.Application.Interfaces;
using Venice.Orders.Domain.Events;

namespace Venice.Orders.Application.Notifications.OrderCreated.Handlers;

public class PublishToRabbitMqHandler : INotificationHandler<OrderCreatedNotification>
{
    private readonly IMessageBus _messageBus;
    private readonly ILogger<PublishToRabbitMqHandler> _logger;

    public PublishToRabbitMqHandler(
        IMessageBus messageBus,
        ILogger<PublishToRabbitMqHandler> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task Handle(OrderCreatedNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            var domainEvent = new OrderCreatedEvent(
                notification.OrderId,
                notification.CustomerId,
                notification.Date,
                notification.Total);

            await _messageBus.PublishAsync(domainEvent, cancellationToken);

            _logger.LogInformation(
                "OrderCreatedEvent published to RabbitMQ for order {OrderId}",
                notification.OrderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error publishing OrderCreatedEvent to RabbitMQ for order {OrderId}",
                notification.OrderId);
        }
    }
}


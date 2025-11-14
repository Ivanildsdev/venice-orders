using MediatR;
using Microsoft.Extensions.Logging;
using Venice.Orders.Application.Interfaces;

namespace Venice.Orders.Application.Notifications.OrderCreated.Handlers;

public class InvalidateCacheHandler : INotificationHandler<OrderCreatedNotification>
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<InvalidateCacheHandler> _logger;

    public InvalidateCacheHandler(
        ICacheService cacheService,
        ILogger<InvalidateCacheHandler> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task Handle(OrderCreatedNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            var cacheKey = $"order:{notification.OrderId}";
            await _cacheService.RemoveAsync(cacheKey, cancellationToken);

            _logger.LogInformation(
                "Cache invalidated for order {OrderId}",
                notification.OrderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error invalidating cache for order {OrderId}",
                notification.OrderId);
        }
    }
}


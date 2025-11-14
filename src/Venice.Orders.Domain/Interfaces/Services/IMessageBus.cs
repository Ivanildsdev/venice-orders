namespace Venice.Orders.Domain.Interfaces.Services;

public interface IMessageBus
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
}


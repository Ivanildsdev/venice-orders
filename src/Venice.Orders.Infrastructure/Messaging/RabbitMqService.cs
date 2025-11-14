using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Venice.Orders.Application.Interfaces;

namespace Venice.Orders.Infrastructure.Messaging;

/// <summary>
/// Implementação do serviço de mensageria usando RabbitMQ - será implementado completamente na Fase 4
/// </summary>
public class RabbitMqService : IMessageBus, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _exchangeName;
    private readonly string _queueName;

    public RabbitMqService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("RabbitMQ");
        var factory = new ConnectionFactory
        {
            Uri = new Uri(connectionString ?? throw new InvalidOperationException("RabbitMQ connection string not configured"))
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        
        _exchangeName = configuration["RabbitMQ:ExchangeName"] ?? "venice.orders";
        _queueName = configuration["RabbitMQ:QueueName"] ?? "pedido.criado";

        // Configurar exchange e queue
        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Topic, durable: true);
        _channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(_queueName, _exchangeName, "pedido.criado");
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.MessageId = Guid.NewGuid().ToString();
        properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        _channel.BasicPublish(
            exchange: _exchangeName,
            routingKey: "pedido.criado",
            basicProperties: properties,
            body: body
        );

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        _channel?.Dispose();
        _connection?.Dispose();
    }
}


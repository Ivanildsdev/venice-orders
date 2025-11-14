namespace Venice.Orders.Application.Interfaces;

/// <summary>
/// Interface para serviço de mensageria (Message Bus)
/// </summary>
public interface IMessageBus
{
    /// <summary>
    /// Publica uma mensagem/evento de forma assíncrona
    /// </summary>
    /// <typeparam name="T">Tipo da mensagem</typeparam>
    /// <param name="message">Mensagem a ser publicada</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
}


namespace Venice.Orders.Application.Interfaces;

/// <summary>
/// Interface para serviço de cache
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Obtém um valor do cache
    /// </summary>
    /// <typeparam name="T">Tipo do objeto a ser recuperado</typeparam>
    /// <param name="key">Chave do cache</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Objeto encontrado ou null se não existir</returns>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Armazena um valor no cache
    /// </summary>
    /// <typeparam name="T">Tipo do objeto a ser armazenado</typeparam>
    /// <param name="key">Chave do cache</param>
    /// <param name="value">Valor a ser armazenado</param>
    /// <param name="expiration">Tempo de expiração</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    Task SetAsync<T>(
        string key, 
        T value, 
        TimeSpan expiration, 
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Remove um valor do cache
    /// </summary>
    /// <param name="key">Chave do cache</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}


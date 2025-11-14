namespace Venice.Orders.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando um pedido possui dados inválidos
/// </summary>
public class InvalidOrderException : DomainException
{
    public InvalidOrderException(string message) : base(message)
    {
    }

    public InvalidOrderException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}


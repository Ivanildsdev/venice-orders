namespace Venice.Orders.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando um item de pedido possui dados inválidos
/// </summary>
public class InvalidOrderItemException : DomainException
{
    public InvalidOrderItemException(string message) : base(message)
    {
    }

    public InvalidOrderItemException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}


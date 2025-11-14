namespace Venice.Orders.Domain.Exceptions;

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


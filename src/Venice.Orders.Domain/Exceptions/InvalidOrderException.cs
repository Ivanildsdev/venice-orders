namespace Venice.Orders.Domain.Exceptions;

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


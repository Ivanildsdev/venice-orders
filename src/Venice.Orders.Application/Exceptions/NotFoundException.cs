namespace Venice.Orders.Application.Exceptions;

/// <summary>
/// Exceção lançada quando um recurso não é encontrado
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}


using FluentValidation.Results;

namespace Venice.Orders.Application.Exceptions;

/// <summary>
/// Exceção lançada quando ocorrem erros de validação
/// </summary>
public class ValidationException : Exception
{
    public IEnumerable<ValidationFailure> Errors { get; }

    public ValidationException(IEnumerable<ValidationFailure> errors)
        : base("One or more validation errors occurred")
    {
        Errors = errors;
    }

    public ValidationException(string message) : base(message)
    {
        Errors = Enumerable.Empty<ValidationFailure>();
    }
}


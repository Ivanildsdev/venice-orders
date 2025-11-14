using FluentValidation.Results;

namespace Venice.Orders.Application.Exceptions;

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


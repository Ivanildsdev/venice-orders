using FluentValidation.TestHelper;
using Venice.Orders.Application.Commands.CreateOrder;

namespace Venice.Orders.Tests.Unit.Application;

public class CreateOrderCommandValidatorTests
{
    [Fact]
    public void Validate_WithValidData_ShouldNotReturnErrors()
    {
        var validator = new CreateOrderCommandValidator();
        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            DateTime.UtcNow.AddSeconds(-1),
            new List<OrderItemRequest>
            {
                new(Guid.NewGuid(), 2, 10.50m)
            }
        );

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}


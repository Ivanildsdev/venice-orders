using MediatR;
using Moq;
using Venice.Orders.Application.Commands.CreateOrder;
using Venice.Orders.Application.DTOs;
using Venice.Orders.Application.Notifications.OrderCreated;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Interfaces;
using Venice.Orders.Domain.Interfaces.Repositories;
using Venice.Orders.Tests.Builders;

namespace Venice.Orders.Tests.Unit.Application;

public class CreateOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidData_ShouldCreateOrderAndPublishNotification()
    {
        var orderRepositoryMock = new Mock<IOrderRepository>();
        var itemRepositoryMock = new Mock<IOrderItemRepository>();
        var mediatorMock = new Mock<IMediator>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        unitOfWorkMock.Setup(u => u.Orders).Returns(orderRepositoryMock.Object);
        unitOfWorkMock.Setup(u => u.Items).Returns(itemRepositoryMock.Object);
        unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            DateTime.UtcNow,
            new List<OrderItemRequest>
            {
                new(Guid.NewGuid(), 2, 10.50m)
            }
        );

        var orderDto = new OrderDto
        {
            Id = Guid.NewGuid(),
            CustomerId = command.CustomerId,
            Total = 21.00m
        };

        var mapper = new AutoMapperBuilder()
            .WithMap<Order, OrderDto>(orderDto)
            .WithMapList<OrderItem, OrderItemDto>(new List<OrderItemDto>())
            .Build();

        var handler = new CreateOrderCommandHandler(
            unitOfWorkMock.Object,
            mediatorMock.Object,
            mapper
        );

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        orderRepositoryMock.Verify(r => r.CreateAsync(
            It.IsAny<Order>(), 
            It.IsAny<CancellationToken>()), Times.Once);
        
        itemRepositoryMock.Verify(r => r.CreateAsync(
            It.IsAny<IEnumerable<OrderItem>>(), 
            It.IsAny<CancellationToken>()), Times.Once);
        
        unitOfWorkMock.Verify(u => u.CommitAsync(
            It.IsAny<CancellationToken>()), Times.Once);
        
        mediatorMock.Verify(m => m.Publish(
            It.IsAny<OrderCreatedNotification>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}


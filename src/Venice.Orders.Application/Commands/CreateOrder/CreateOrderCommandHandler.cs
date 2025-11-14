using AutoMapper;
using MediatR;
using Venice.Orders.Application.DTOs;
using Venice.Orders.Application.Notifications.OrderCreated;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Interfaces;

namespace Venice.Orders.Application.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order(request.CustomerId, request.Date);

        foreach (var itemRequest in request.Items)
        {
            order.AddItem(
                itemRequest.ProductId,
                itemRequest.Quantity,
                itemRequest.UnitPrice
            );
        }

        await _unitOfWork.Orders.CreateAsync(order, cancellationToken);

        var items = order.Items.Select(i => new OrderItem(
            order.Id,
            i.ProductId,
            i.Quantity,
            i.UnitPrice
        )).ToList();
        
        await _unitOfWork.Items.CreateAsync(items, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        var notification = new OrderCreatedNotification(
            order.Id,
            order.CustomerId,
            order.Date,
            order.Total);
        
        await _mediator.Publish(notification, cancellationToken);

        var orderDto = _mapper.Map<OrderDto>(order);
        orderDto.Items = _mapper.Map<List<OrderItemDto>>(items);
        
        return orderDto;
    }
}


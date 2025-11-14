using AutoMapper;
using MediatR;
using Venice.Orders.Application.DTOs;
using Venice.Orders.Application.Interfaces;
using Venice.Orders.Domain.Interfaces;

namespace Venice.Orders.Application.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"order:{request.Id}";
        var cachedOrder = await _cacheService.GetAsync<OrderDto>(cacheKey, cancellationToken);

        if (cachedOrder != null)
            return cachedOrder;

        var order = await _unitOfWork.Orders.GetByIdAsync(request.Id, cancellationToken);

        if (order == null)
            return null;

        var items = await _unitOfWork.Items.GetByOrderIdAsync(request.Id, cancellationToken);

        var orderDto = _mapper.Map<OrderDto>(order);
        orderDto.Items = _mapper.Map<List<OrderItemDto>>(items);

        await _cacheService.SetAsync(cacheKey, orderDto, TimeSpan.FromMinutes(2), cancellationToken);

        return orderDto;
    }
}


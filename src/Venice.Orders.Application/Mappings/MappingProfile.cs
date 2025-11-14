using AutoMapper;
using Venice.Orders.Application.DTOs;
using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Application.Mappings;

/// <summary>
/// Profile de mapeamento AutoMapper
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<OrderItem, OrderItemDto>();
    }
}


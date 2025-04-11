using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OrderAddRequest, Order>();
        CreateMap<OrderItemAddRequest, OrderItem>();
        
        CreateMap<OrderUpdateRequest, Order>();
        CreateMap<OrderItemUpdateRequest, OrderItem>();
        
        CreateMap<Order, OrderResponse>();
        CreateMap<OrderItem, OrderItemResponse>();
        
        CreateMap<ProductDTO, OrderItemResponse>();
    }
}
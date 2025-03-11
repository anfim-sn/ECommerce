using AutoMapper;
using ECommerce.Core.DTO;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductRequest, ProductResponse>().ForMember(dest => dest.IsSuccess, opt => opt.Ignore());
        CreateMap<Product, ProductResponse>().ForMember(dest => dest.IsSuccess, opt => opt.Ignore());
        CreateMap<ProductRequest, Product>();
        ;
    }
}
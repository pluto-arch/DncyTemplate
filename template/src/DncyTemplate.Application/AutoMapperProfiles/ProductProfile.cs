using AutoMapper;
using DncyTemplate.Application.Command.Product;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Domain.Aggregates.Product;

namespace DncyTemplate.Application.AutoMapperProfiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(x => x.Id, o => o.MapFrom(z => z.Id))
            .ForMember(x => x.Name, o => o.MapFrom(z => z.Name))
            .ForMember(x => x.Remark, o => o.MapFrom(z => z.Remark))
            .ForMember(x => x.CreateTime, o => o.MapFrom(z => z.CreationTime.DateTime));

        CreateMap<Product, ProductListItemDto>()
            .ForMember(x => x.Id, o => o.MapFrom(z => z.Id))
            .ForMember(x => x.Name, o => o.MapFrom(z => z.Name))
            .ForMember(x => x.CreateTime, o => o.MapFrom(z => z.CreationTime.DateTime));

        CreateMap<ProductCreateRequest, Product>();


        CreateMap<CreateProductCommand, Product>();


        CreateMap<ProductUpdateRequest, Product>();
    }
}
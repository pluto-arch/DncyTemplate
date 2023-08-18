using Dncy.MultiTenancy;
using DncyTemplate.Application.Command.Product;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Domain.Aggregates.Product;
using Mapster;

namespace DncyTemplate.Application.MapperProfiles;

public class ProductMapper : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => MapFormatedName(src.Name))
            .Map(dest => dest.Remark, src => src.Remark)
            .Map(dest => dest.CreateTime, src => src.CreationTime.DateTime);

        config.NewConfig<Product, ProductListItemDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.CreateTime, src => src.CreationTime.DateTime);

        config.NewConfig<ProductCreateRequest, Product>();
        config.NewConfig<CreateProductCommand, Product>()
            .Map(dest => dest.Name, src => src.Name);
        config.NewConfig<ProductUpdateRequest, Product>();
    }

    string MapFormatedName(string originName)
    {
        // customer formet value
        var config = MapContext.Current.GetService<ICurrentTenant>();
        return $"{originName}_{config.Id}";
    }
}
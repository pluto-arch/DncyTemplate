using DncyTemplate.Application.AppServices.Generics;
using DncyTemplate.Application.Models.Product;

namespace DncyTemplate.Application.AppServices.Product;

public interface IProductAppService
    : ICrudAppService<string, ProductDto, ProductPagedRequest, ProductListItemDto, ProductUpdateRequest, ProductCreateRequest>
{
    
}
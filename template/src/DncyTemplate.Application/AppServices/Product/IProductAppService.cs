using DncyTemplate.Application.AppServices.Generics;
using DncyTemplate.Application.Models;
using DncyTemplate.Application.Models.Product;

namespace DncyTemplate.Application.AppServices.Product;

public interface IProductAppService
    : ICrudAppService<string, ProductDto, ProductPagedRequest, ProductListItemDto, ProductUpdateRequest, ProductCreateRequest>
{
    Task<Return<ProductDto,string>> GetByName(string productName);
}
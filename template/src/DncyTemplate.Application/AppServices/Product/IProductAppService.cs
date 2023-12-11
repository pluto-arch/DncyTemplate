using DncyTemplate.Application.AppServices.Generics;
using DncyTemplate.Application.Models.Product;
using Dotnetydd.Tools.Core.Models;

namespace DncyTemplate.Application.AppServices.Product;

public interface IProductAppService
    : ICrudAppService<string, ProductDto, ProductPagedRequest, ProductListItemDto, ProductUpdateRequest, ProductCreateRequest>
{
    Task<Return<ProductDto, string>> GetByName(string productName);

    Task<Return<ProductDto, string>> GetByNameWithDapper(string productName);
}
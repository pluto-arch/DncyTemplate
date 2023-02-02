using DncyTemplate.Application.Models.Product;

namespace DncyTemplate.Application.Queries.Product
{
    public interface IProductQueries
    {
        Task<ProductDto> GetAsync(string id);
    }
}

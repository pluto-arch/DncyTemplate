
using Microsoft.Extensions.DependencyInjection;

namespace DncyTemplate.Application.AppServices.Product;

[Injectable(InjectLifeTime.Scoped,typeof(IProductAppService))]
public class ProductAppService:IProductAppService
{
    
}
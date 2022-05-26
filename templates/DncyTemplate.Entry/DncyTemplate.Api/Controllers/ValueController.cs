using Dncy.MultiTenancy;
using Dncy.MultiTenancy.Model;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Repository;
using DncyTemplate.Domain.UnitOfWork;

namespace DncyTemplate.Api.Controllers;

[Route("api/[controller]")]
[AutoResolveDependency]
[ApiController]
public partial class ValueController : ControllerBase
{

    [AutoInject]
    private readonly ICurrentTenant _currentTenant;

    [AutoInject]
    private readonly IRepository<Product> _productsRepository;


    [HttpGet]
    public async Task<IEnumerable<Product>> Get()
    {
        return await _productsRepository.GetListAsync();
    }

    [HttpGet("tenant")]
    public TenantInfo GeTenantInfo()
    {
        return new TenantInfo(_currentTenant.Id, _currentTenant.Name);
    }
}
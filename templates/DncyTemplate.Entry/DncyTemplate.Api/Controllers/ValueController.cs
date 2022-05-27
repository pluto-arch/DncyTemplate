using Dncy.MultiTenancy;
using Dncy.MultiTenancy.Model;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Repository;

namespace DncyTemplate.Api.Controllers;

/// <summary>
/// 示例控制器
/// </summary>
[Route("api/[controller]")]
[AutoResolveDependency]
[ApiController]
public partial class ValueController : ControllerBase
{

    [AutoInject]
    private readonly ICurrentTenant _currentTenant;

    [AutoInject]
    private readonly IRepository<Product> _productsRepository;


    /// <summary>
    /// 获取产品列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<Product>> Get()
    {
        return await _productsRepository.GetListAsync();
    }

    /// <summary>
    /// 获取租户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("tenant")]
    public TenantInfo GeTenantInfo()
    {
        return new TenantInfo(_currentTenant.Id, _currentTenant.Name);
    }
}
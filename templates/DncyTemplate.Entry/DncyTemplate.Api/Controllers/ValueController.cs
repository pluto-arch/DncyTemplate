using Dncy.MultiTenancy;
using Dncy.MultiTenancy.Model;
using Microsoft.AspNetCore.Mvc;

namespace DncyTemplate.Api.Controllers
{

    [Route("api/[controller]")]
    [AutoResolveDependency]
    [ApiController]
    public partial class ValueController : ControllerBase
    {

        [AutoInject]
        private readonly ICurrentTenant _currentTenant;


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Enumerable.Repeat<string>("111", 200);
        }

        [HttpGet("tenant")]
        public TenantInfo GeTenantInfo()
        {
            return new TenantInfo(_currentTenant.Id, _currentTenant.Name);
        }
    }
}
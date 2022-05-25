using Dncy.MultiTenancy;
using Dncy.MultiTenancy.ConnectionStrings;
using Dncy.MultiTenancy.Store;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DncyTemplate.Domain
{
    public static class DependencyInject
    {
        public static IServiceCollection AddDomainModule(this IServiceCollection service,IConfiguration configuration)
        {

            
            return service;
        }
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DncyTemplate.Application
{
    public static class DependencyInject
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection service,IConfiguration _)
        {
            return service;
        }
    }
}
using DncyTemplate.Domain.Infra;
using DncyTemplate.Infra.EntityFrameworkCore;
using DncyTemplate.Infra.Providers;

namespace DncyTemplate.Infra
{
    public static class DependencyInject
    {
        public static IServiceCollection AddInfraModule(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddTransient<IDomainEventDispatcher, MediatrDomainEventDispatcher>();
            service.AddEfCoreInfraComponent(configuration);
            return service;
        }

    }
}
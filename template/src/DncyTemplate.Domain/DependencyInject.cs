using DncyTemplate.Uow;

namespace DncyTemplate.Domain
{
    public static class DependencyInject
    {
        public static IServiceCollection AddDomainModule(this IServiceCollection service)
        {
            service.AutoInjectDncyTemplate_Domain();
            return service;
        }
    }
}
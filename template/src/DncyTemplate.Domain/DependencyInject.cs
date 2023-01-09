using DncyTemplate.Domain.UnitOfWork;

namespace DncyTemplate.Domain
{
    public static class DependencyInject
    {
        public static IServiceCollection AddDomainModule(this IServiceCollection service)
        {
            service.AddScoped<UnitOfWorkScopeManager>();

            service.AutoInjectDncyTemplate_Domain();

            return service;
        }
    }
}
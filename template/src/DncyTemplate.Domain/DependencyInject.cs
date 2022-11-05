using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.UnitOfWork;

using Microsoft.Extensions.DependencyInjection;

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
using DncyTemplate.Domain.Infra;
using DncyTemplate.Infra.EntityFrameworkCore;
using DncyTemplate.Infra.Global;
using DncyTemplate.Infra.Providers;

namespace DncyTemplate.Infra
{
    public static class DependencyInject
    {
        public static IServiceCollection AddInfraModule(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddSingleton<GlobalAccessor.CurrentUserAccessor>();
            service.AddScoped<GlobalAccessor.CurrentUser>();

            var ctxs = GetDbContextTypes();
            DataContextTypeCache.AddDataContext(ctxs);
            service.AddTransient<IDomainEventDispatcher, MediatrDomainEventDispatcher>();
            service.AddEfCoreInfraComponent(configuration, ctxs);
            service.AddEfUnitofWorkWithAccessor(ctxs);
            return service;
        }

        public static List<Type> GetDbContextTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAssignableTo(typeof(DbContext)) && !x.IsAbstract && !x.Name.Contains("Migration")).ToList();
        }
    }
}
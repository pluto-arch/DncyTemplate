using DncyTemplate.Domain.Infra;
using DncyTemplate.Infra.EntityFrameworkCore;
using DncyTemplate.Infra.Providers;

namespace DncyTemplate.Infra
{
    public static class DependencyInject
    {
        public static IServiceCollection AddInfraModule(this IServiceCollection service, IConfiguration configuration)
        {

            var ctxs = GetDbContextTypes();


            service.AddTransient<IDomainEventDispatcher, MediatrDomainEventDispatcher>();
            service.AddEfCoreInfraComponent(configuration, ctxs);
            service.AddEfUnitofWork(ctxs);
            DataContextTypeCache.AddDataContext(ctxs);
            return service;
        }


        public static List<Type> GetDbContextTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAssignableTo(typeof(DbContext)) && !x.IsAbstract && !x.Name.Contains("Migration")).ToList();
        }
    }
}
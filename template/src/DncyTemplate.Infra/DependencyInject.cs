using DncyTemplate.Constants;
using DncyTemplate.Domain.Infra;
using DncyTemplate.Infra.EntityFrameworkCore;
using DncyTemplate.Infra.EntityFrameworkCore.ConnectionStringResolve;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Infra.EntityFrameworkCore.Interceptor;
using DncyTemplate.Infra.Global;
using DncyTemplate.Infra.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace DncyTemplate.Infra
{
    public static class DependencyInject
    {
        public static IServiceCollection AddInfraModule(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddSingleton<GlobalAccessor.CurrentUserAccessor>();
            service.AddScoped<GlobalAccessor.CurrentUser>();
            service.AddTransient<IDomainEventDispatcher, MediatrDomainEventDispatcher>();




            #region DncyTemplate DbContext

            service.AddKeyedSingleton<IConnectionStringResolve, DefaultConnectionStringResolve>(nameof(DncyTemplateDbContext));
            var migrationAssembly = Assembly.GetCallingAssembly().GetName().Name;
            service.AddEfCoreInfraComponent<DncyTemplateDbContext>((serviceProvider,optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString(InfraConstantValue.DEFAULT_CONNECTIONSTRING_NAME),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(migrationAssembly);
                    });

                var mediator = serviceProvider.GetService<IDomainEventDispatcher>() ?? NullDomainEventDispatcher.Instance;
                optionsBuilder.AddInterceptors(new DataChangeSaveChangesInterceptor(mediator));

#if Tenant
                //多租户模式下解析租户连接字符串使用
                var connectionStringResolve = serviceProvider.GetKeyedService<IConnectionStringResolve>(nameof(DncyTemplateDbContext));
                optionsBuilder.AddInterceptors(new TenantDbConnectionInterceptor(connectionStringResolve, InfraConstantValue.DEFAULT_CONNECTIONSTRING_NAME));
#endif

#if DEBUG
                optionsBuilder.EnableSensitiveDataLogging();
#endif
            });
            service.AddEfUnitofWorkWithAccessor<DncyTemplateDbContext>();

            #endregion



            return service;
        }
    }
}
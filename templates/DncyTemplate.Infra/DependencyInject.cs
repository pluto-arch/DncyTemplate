using Dncy.Specifications.EntityFrameworkCore;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Repository;
using DncyTemplate.Domain.UnitOfWork;
using DncyTemplate.Infra.Constants;
using DncyTemplate.Infra.EntityFrameworkCore.ConnectionStringResolve;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Infra.EntityFrameworkCore.Interceptor;
using DncyTemplate.Infra.EntityFrameworkCore.Repositories;
using DncyTemplate.Infra.EntityFrameworkCore.UnitOfWork;
using DncyTemplate.Infra.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using System.Reflection;

namespace DncyTemplate.Infra
{
    public static class DependencyInject
    {
        public static IServiceCollection AddInfraModule(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddTransient<IDomainEventDispatcher, MediatrDomainEventDispatcher>();

            service.AddEntityFrameworkSqlServer();
            service.AddSingleton<IConnectionStringResolve, DefaultConnectionStringResolve>();
            service.AddDbContextPool<DeviceCenterDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString(DbConstants.DEFAULT_CONNECTIONSTRING_NAME),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                        sqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
                    });

                var mediator = serviceProvider.GetService<IDomainEventDispatcher>() ?? NullDomainEventDispatcher.Instance;
                optionsBuilder.AddInterceptors(new DataChangeSaveChangesInterceptor(mediator));
                //多租户模式下解析租户连接字符串使用
                var connectionStringResolve = serviceProvider.GetRequiredService<IConnectionStringResolve>();
                optionsBuilder.AddInterceptors(new TenantDbConnectionInterceptor(connectionStringResolve, DbConstants.DEFAULT_CONNECTIONSTRING_NAME));

                optionsBuilder.UseInternalServiceProvider(serviceProvider);
#if DEBUG
                optionsBuilder.EnableSensitiveDataLogging();
#endif
            });

            //分表使用 - 替换ef的缓存，造成性能会下降 并且不能使用 AddDbContextPool.
            //service.Replace(ServiceDescriptor.Singleton<IModelCacheKeyFactory, DeviceCenterDbContext.DynamicModelCacheKeyFactory>());

            ApplyEntityDefaultNavicationProperty(service);
            AddUnitofWork(service);
            service.AddDefaultRepository();
            return service;
        }


        private static void ApplyEntityDefaultNavicationProperty(IServiceCollection service)
        {
            // 设置实体默认显示加载的导航属性
            service.Configure<IncludeRelatedPropertiesOptions>(options =>
            {
                options.ConfigIncludes<Product>(e => e.Include(d => d.Devices).ThenInclude(f => f.Address));
            });
        }


        private static void AddUnitofWork(IServiceCollection service)
        {
            service.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            var assembly = Assembly.GetExecutingAssembly();
            var context = assembly.GetTypes()
                .Where(x => x.GetInterface(nameof(IUowDbContext)) != null && !x.Name.Contains("Migration")).ToList();
            service.Configure<UnitOfWorkCollectionOptions>(s =>
            {
                foreach (var item in context)
                {
                    var uowType = typeof(IUnitOfWork<>).MakeGenericType(item);
                    s.DbContexts.Add(item.Name, uowType);
                }
            });
        }


        public static void AddDefaultRepository(this IServiceCollection services, Assembly assembly = null,
            List<Type> context = null)
        {
            assembly ??= Assembly.GetExecutingAssembly();
            context ??= assembly.GetTypes().Where(x => x.GetInterface(nameof(IUowDbContext)) != null).ToList();
            if (context is null or { Count: <= 0 })
            {
                return;
            }

            foreach (var item in context)
            {
                var properties = item.GetProperties().Where(x => x.PropertyType.IsGenericType);
                if (!properties.Any())
                {
                    Log.Logger.Warning($"DbContext does not have any entity properties for default repository inject");
                }
                foreach (var p in properties)
                {
                    var entityType = p.PropertyType.GenericTypeArguments.FirstOrDefault(x => x.IsAssignableTo(typeof(IEntity)));
                    if (entityType == null)
                    {
                        continue;
                    }

                    var baseImpl = typeof(BaseRepository<,>).MakeGenericType(item, entityType);
                    var baseRep = typeof(IRepository<>).MakeGenericType(entityType);
                    services.RegisterType(baseRep, baseImpl);
                    var primaryKeyType = EntityHelper.FindPrimaryKeyType(entityType);
                    if (primaryKeyType != null)
                    {
                        var keyImpl = typeof(BaseRepository<,,>).MakeGenericType(item, entityType, primaryKeyType);
                        var keyRep = typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType);
                        services.RegisterType(keyRep, keyImpl);
                    }
                }
            }
        }

        public static IServiceCollection RegisterType(this IServiceCollection services, Type type, Type implementationType)
        {
            if (type.IsAssignableFrom(implementationType))
            {
                services.TryAddTransient(type, implementationType);
            }

            return services;
        }



    }
}
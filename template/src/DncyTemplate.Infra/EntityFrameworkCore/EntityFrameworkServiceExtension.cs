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

namespace DncyTemplate.Infra.EntityFrameworkCore;

public static class EntityFrameworkServiceExtension
{

    /// <summary>
    /// 添加efcore 组件
    /// </summary>
    /// <param name="service"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    internal static IServiceCollection AddEfCoreInfraComponent(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddEntityFrameworkSqlServer();
        service.AddSingleton<IConnectionStringResolve, DefaultConnectionStringResolve>();
        service.AddDbContext<DncyTemplateDbContext>((serviceProvider, optionsBuilder) =>
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

#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
#endif
        });


        service.AddUnitofWork();
        service.AddDefaultRepository();
        service.ApplyEntityDefaultNavicationProperty();
        return service;
    }



    #region private

    private static void ApplyEntityDefaultNavicationProperty(this IServiceCollection service)
    {
        // 设置实体默认显示加载的导航属性
        service.Configure<IncludeRelatedPropertiesOptions>(options =>
        {
            options.ConfigIncludes<Product>(e => e.Include(d => d.Devices).ThenInclude(f => f.Address));
        });
    }



    private static void AddUnitofWork(this IServiceCollection service)
    {
        service.AddScoped(typeof(IUnitOfWork<>), typeof(EfCoreUnitOfWork<>));
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



    private static void AddDefaultRepository(this IServiceCollection services, Assembly assembly = null, List<Type> context = null)
    {
        assembly ??= Assembly.GetExecutingAssembly();
        context ??= assembly.GetTypes().Where(x => x.GetInterface(nameof(IUowDbContext)) != null).ToList();
        if (context is null or { Count: <= 0 })
        {
            return;
        }

        Parallel.ForEach(context, item =>
        {
            var properties = item.GetProperties().Where(x => x.PropertyType.IsGenericType);
            if (!properties.Any())
            {
                Log.Logger.Warning("{Name} does not have any entity properties for default repository inject", item.Name);
                return;
            }
            foreach (var p in properties)
            {
                var entityType = p.PropertyType.GenericTypeArguments.FirstOrDefault(x => x.IsAssignableTo(typeof(IEntity)));
                if (entityType == null)
                {
                    continue;
                }

                var baseImpl = typeof(EfCoreBaseRepository<,>).MakeGenericType(item, entityType);
                var baseRep = typeof(IRepository<>).MakeGenericType(entityType);
                services.RegisterType(baseRep, baseImpl);

                var primaryKeyType = EntityHelper.FindPrimaryKeyType(entityType);
                if (primaryKeyType != null)
                {
                    var keyImpl = typeof(EfCoreBaseRepository<,,>).MakeGenericType(item, entityType, primaryKeyType);
                    var keyRep = typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType);
                    services.RegisterType(keyRep, keyImpl);
                }
            }
        });
    }

    private static IServiceCollection RegisterType(this IServiceCollection services, Type type, Type implementationType)
    {
        if (type.IsAssignableFrom(implementationType))
        {
            services.TryAddTransient(type, implementationType);
        }

        return services;
    }
    #endregion

}
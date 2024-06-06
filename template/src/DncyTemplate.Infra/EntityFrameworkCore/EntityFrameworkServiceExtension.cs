using DncyTemplate.Constants;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Infra.Repository;
using DncyTemplate.Infra.EntityFrameworkCore.ConnectionStringResolve;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Infra.EntityFrameworkCore.Interceptor;
using DncyTemplate.Uow;
using DncyTemplate.Uow.EntityFrameworkCore;
using Dotnetydd.Specifications.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DncyTemplate.Infra.EntityFrameworkCore;

public static class EntityFrameworkServiceExtension
{

    /// <summary>
    /// 添加efcore 组件
    /// </summary>
    /// <returns></returns>
    internal static IServiceCollection AddEfCoreInfraComponent(this IServiceCollection service, IConfiguration configuration, List<Type> contextTypes)
    {
        service.AddSingleton<IConnectionStringResolve, DefaultConnectionStringResolve>();
        var migrationAssembly = Assembly.GetCallingAssembly().GetName().Name;
        service.AddDbContextFactory<DncyTemplateDbContext>((serviceProvider, optionsBuilder) =>
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
            var connectionStringResolve = serviceProvider.GetRequiredService<IConnectionStringResolve>();
            optionsBuilder.AddInterceptors(new TenantDbConnectionInterceptor(connectionStringResolve, InfraConstantValue.DEFAULT_CONNECTIONSTRING_NAME));
#endif


#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
#endif
        }, ServiceLifetime.Scoped);

        service.AddDefaultRepository(contextTypes);
        service.ApplyEntityDefaultNavicationProperty();
        return service;
    }


    public static void AddEfUnitofWorkWithAccessor(this IServiceCollection services, List<Type> context = null)
    {
        if (context is null or { Count: <= 0 })
        {
            return;
        }

        if (context.Count == 1)
        {
            var defType = typeof(IUnitOfWork);
            var defType2 = typeof(EfUnitOfWork<>).MakeGenericType(context[0]);
            services.RegisterScopedType(defType, defType2);
        }

        foreach (var item in context)
        {
            var defType = typeof(IUnitOfWork<>).MakeGenericType(item);
            var defType2 = typeof(EfUnitOfWork<>).MakeGenericType(item);
            services.RegisterScopedType(defType, defType2);
        }
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



    private static void AddDefaultRepository(this IServiceCollection services, List<Type> context = null)
    {
        if (context is null or { Count: <= 0 })
        {
            return;
        }

        Parallel.ForEach(context, item =>
        {
            var entitTypies =
                from property in item.GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                where IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) &&
                      typeof(IEntity).IsAssignableFrom(property.PropertyType.GenericTypeArguments[0])
                select property.PropertyType.GenericTypeArguments[0];


            foreach (var entityType in entitTypies)
            {
                var defType = typeof(IEfRepository<>).MakeGenericType(entityType);
                var defType2 = typeof(IEfContextRepository<,>).MakeGenericType(item, entityType);
                var implementingType = EfRepositoryHelper.GetRepositoryType(item, entityType);
                services.RegisterScopedType(defType, implementingType);
                services.RegisterScopedType(defType2, implementingType);

                Type keyType = EntityHelper.FindPrimaryKeyType(entityType);
                if (keyType != null)
                {
                    var impl = EfRepositoryHelper.GetRepositoryType(item, entityType, keyType);
                    services.RegisterScopedType(typeof(IEfRepository<,>).MakeGenericType(entityType, keyType), impl);
                    services.RegisterScopedType(typeof(IEfContextRepository<,,>).MakeGenericType(item, entityType, keyType),
                        impl);
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


    private static IServiceCollection RegisterScopedType(this IServiceCollection services, Type type, Type implementationType)
    {
        if (type.IsAssignableFrom(implementationType))
        {
            services.TryAddScoped(type, implementationType);
        }
        return services;
    }

    public static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        TypeInfo typeInfo = givenType.GetTypeInfo();
        if (typeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        Type[] interfaces = typeInfo.GetInterfaces();
        foreach (Type type in interfaces)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
        }

        if (typeInfo.BaseType == null)
        {
            return false;
        }

        return IsAssignableToGenericType(typeInfo.BaseType, genericType);
    }
    #endregion
}
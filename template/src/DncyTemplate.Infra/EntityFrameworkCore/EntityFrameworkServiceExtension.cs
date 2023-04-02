using Dncy.Specifications.EntityFrameworkCore;
using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Infra;
using DncyTemplate.Infra.Constants;
using DncyTemplate.Infra.EntityFrameworkCore.ConnectionStringResolve;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Infra.EntityFrameworkCore.Interceptor;
using DncyTemplate.Infra.EntityFrameworkCore.Repository;

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
        service.AddSingleton<IConnectionStringResolve, DefaultConnectionStringResolve>();
        service.AddDbContextPool<DncyTemplateDbContext>((serviceProvider, optionsBuilder) =>
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(DbConstants.DEFAULT_CONNECTIONSTRING_NAME),
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    //sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromMilliseconds(400), null);
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
        service.AddScoped(typeof(EfUnitOfWork<>));
    }



    private static void AddDefaultRepository(this IServiceCollection services, Assembly assembly = null, List<Type> context = null)
    {
        assembly ??= Assembly.GetExecutingAssembly();
        context ??= assembly.GetTypes().Where(x => x.IsAssignableTo(typeof(DbContext)) && !x.Name.Contains("Migration")).ToList();
        if (context is null or { Count: <= 0 })
        {
            return;
        }


        Parallel.ForEach(context, item =>
        {
            var entitTypies = from property in item.GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                              where IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) && typeof(IEntity).IsAssignableFrom(property.PropertyType.GenericTypeArguments[0])
                              select property.PropertyType.GenericTypeArguments[0];


            foreach (var entityType in entitTypies)
            {
                var defType = typeof(IEfRepository<>).MakeGenericType(entityType);
                var implementingType = EfRepositoryHelper.GetRepositoryType(item, entityType);
                services.RegisterType(defType, implementingType);

                Type keyType = EntityHelper.FindPrimaryKeyType(entityType);
                if (keyType != null)
                {
                    services.RegisterType(typeof(IEfRepository<,>).MakeGenericType(entityType, keyType),
                        EfRepositoryHelper.GetRepositoryType(item, entityType, keyType));
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
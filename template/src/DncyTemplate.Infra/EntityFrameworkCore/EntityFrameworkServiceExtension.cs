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
    /// <remarks>包括efcore dbcontext，默认仓储</remarks>
    /// <returns></returns>
    public static IServiceCollection AddEfCoreInfraComponent<TDbContext>(this IServiceCollection service, 
        Action<IServiceProvider,DbContextOptionsBuilder> optionsAction)
        where TDbContext:DbContext
    {
        service.AddDbContextFactory<TDbContext>(optionsAction, ServiceLifetime.Scoped);
        service.AddDefaultRepository<TDbContext>();
        return service;
    }

    /// <summary>
    /// 添加默认仓储
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    internal static void AddDefaultRepository<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        var item = typeof(TDbContext);

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
    }



    public static void AddEfUnitofWorkWithAccessor<TDbContext>(this IServiceCollection services)
        where TDbContext:DbContext
    {
        var dbcontextType = typeof(TDbContext);
        services.RegisterScopedType(typeof(IUnitOfWork), typeof(EfUnitOfWork<>).MakeGenericType(dbcontextType));
        services.RegisterScopedType(typeof(IUnitOfWork<>).MakeGenericType(dbcontextType), typeof(EfUnitOfWork<>).MakeGenericType(dbcontextType));
    }


    #region private


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
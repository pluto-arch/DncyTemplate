using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace DncyTemplate.Domain.Infra;

public static partial class EntityHelper
{
    public static bool IsMultiTenant<TEntity>()
        where TEntity : IEntity
    {
        return IsMultiTenant(typeof(TEntity));
    }

    public static bool IsMultiTenant(Type type)
    {
        return typeof(IMultiTenant).IsAssignableFrom(type);
    }


    public static bool EntityEquals(IEntity entity1, IEntity entity2)
    {
        if (entity1 == null || entity2 == null)
        {
            return false;
        }

        //Same instances must be considered as equal
        if (ReferenceEquals(entity1, entity2))
        {
            return true;
        }

        //Must have a IS-A relation of types or must be same type
        Type typeOfEntity1 = entity1.GetType();
        Type typeOfEntity2 = entity2.GetType();
        if (!typeOfEntity1.IsAssignableFrom(typeOfEntity2) && !typeOfEntity2.IsAssignableFrom(typeOfEntity1))
        {
            return false;
        }

        //Different tenants may have an entity with same Id.
        if (entity1 is IMultiTenant tenant1 && entity2 is IMultiTenant tenant2)
        {
            string tenant1Id = tenant1.TenantId;
            string tenant2Id = tenant2.TenantId;

            if (tenant1Id != tenant2Id)
            {
                if (tenant1Id == null || tenant2Id == null)
                {
                    return false;
                }

                if (!tenant1Id.Equals(tenant2Id))
                {
                    return false;
                }
            }
        }

        //Transient objects are not considered as equal
        if (HasDefaultKeys(entity1) && HasDefaultKeys(entity2))
        {
            return false;
        }

        object[] entity1Keys = entity1.GetKeys();
        object[] entity2Keys = entity2.GetKeys();

        if (entity1Keys.Length != entity2Keys.Length)
        {
            return false;
        }

        for (int i = 0; i < entity1Keys.Length; i++)
        {
            object entity1Key = entity1Keys[i];
            object entity2Key = entity2Keys[i];

            if (entity1Key == null)
            {
                if (entity2Key == null)
                {
                    //Both null, so considered as equals
                    continue;
                }

                //entity2Key is not null!
                return false;
            }

            if (entity2Key == null)
            {
                //entity1Key was not null!
                return false;
            }

            if (IsDefaultValue(entity1Key) && IsDefaultValue(entity2Key))
            {
                return false;
            }

            if (!entity1Key.Equals(entity2Key))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsEntity([NotNull] Type type)
    {
        ValueCheck.NotNull(type, nameof(type));
        return typeof(IEntity).IsAssignableFrom(type);
    }

    public static void CheckEntity([NotNull] Type type)
    {
        ValueCheck.NotNull(type, nameof(type));
        if (!IsEntity(type))
        {
            throw new Exception(
                $"Given {nameof(type)} is not an entity: {type.AssemblyQualifiedName}. It must implement {typeof(IEntity).AssemblyQualifiedName}.");
        }
    }

    public static bool IsEntityWithId([NotNull] Type type)
    {
        foreach (Type interfaceType in type.GetInterfaces())
        {
            if (interfaceType.GetTypeInfo().IsGenericType &&
                interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasDefaultId<TKey>(IEntity<TKey> entity)
    {
        if (EqualityComparer<TKey>.Default.Equals(entity.Id, default))
        {
            return true;
        }

        //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
        if (typeof(TKey) == typeof(int))
        {
            return Convert.ToInt32(entity.Id) <= 0;
        }

        if (typeof(TKey) == typeof(long))
        {
            return Convert.ToInt64(entity.Id) <= 0;
        }

        return false;
    }

    private static bool IsDefaultKeyValue(object value)
    {
        if (value == null)
        {
            return true;
        }

        Type type = value.GetType();

        //Workaround for EF Core since it sets int/long to min value when attaching to DbContext
        if (type == typeof(int))
        {
            return Convert.ToInt32(value) <= 0;
        }

        if (type == typeof(long))
        {
            return Convert.ToInt64(value) <= 0;
        }

        return IsDefaultValue(value);
    }

    public static bool HasDefaultKeys([NotNull] IEntity entity)
    {
        ValueCheck.NotNull(entity, nameof(entity));

        foreach (object key in entity.GetKeys())
        {
            if (!IsDefaultKeyValue(key))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    ///     Tries to find the primary key type of the given entity type.
    ///     May return null if given type does not implement <see cref="IEntity{TKey}" />
    /// </summary>
    public static Type FindPrimaryKeyType<TEntity>()
        where TEntity : IEntity
    {
        return FindPrimaryKeyType(typeof(TEntity));
    }

    /// <summary>
    ///     Tries to find the primary key type of the given entity type.
    ///     May return null if given type does not implement <see cref="IEntity{TKey}" />
    /// </summary>
    public static Type FindPrimaryKeyType([NotNull] Type entityType)
    {
        if (!typeof(IEntity).IsAssignableFrom(entityType))
        {
            throw new Exception(
                $"Given {nameof(entityType)} is not an entity. It should implement {typeof(IEntity).AssemblyQualifiedName}!");
        }

        foreach (Type interfaceType in entityType.GetTypeInfo().GetInterfaces())
        {
            if (interfaceType.GetTypeInfo().IsGenericType &&
                interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
            {
                return interfaceType.GenericTypeArguments[0];
            }
        }

        return null;
    }

    public static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId<TEntity, TKey>(TKey id)
        where TEntity : IEntity<TKey>
    {
        ParameterExpression lambdaParam = Expression.Parameter(typeof(TEntity));
        MemberExpression leftExpression = Expression.PropertyOrField(lambdaParam, "Id");
        object idValue = Convert.ChangeType(id, typeof(TKey));
        Expression<Func<object>> closure = () => idValue;
        UnaryExpression rightExpression = Expression.Convert(closure.Body, leftExpression.Type);
        BinaryExpression lambdaBody = Expression.Equal(leftExpression, rightExpression);
        return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
    }

    public static bool IsDefaultValue(object obj)
    {
        if (obj == null)
        {
            return true;
        }

        return obj.Equals(GetDefaultValue(obj.GetType()));
    }

    public static T GetDefaultValue<T>()
    {
        return default;
    }

    public static object GetDefaultValue(Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }

        return null;
    }
}
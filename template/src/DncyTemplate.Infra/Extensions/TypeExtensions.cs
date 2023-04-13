using System.Runtime.Serialization;

namespace DncyTemplate.Infra.Extensions;

public static class TypeExtensions
{
    public static string GetGenericTypeName(this Type type)
    {
        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name));
            return $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        return type.Name;
    }

    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetGenericTypeName();
    }


    public static bool IsNullOrEmpty<T>(this ICollection<T> source) => source == null || source.Count <= 0;

    /// <summary>
    /// Tries to parse a string into an enum honoring EnumMemberAttribute if present
    /// </summary>
    public static bool TryParseWithMemberName<TEnum>(this string value, out TEnum result) where TEnum : struct
    {
        result = default;

        if (string.IsNullOrEmpty(value))
            return false;

        Type enumType = typeof(TEnum);

        foreach (string name in Enum.GetNames(typeof(TEnum)))
        {
            if (name.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                result = Enum.Parse<TEnum>(name);
                return true;
            }

            var memberAttribute = enumType.GetField(name)?.GetCustomAttribute(typeof(EnumMemberAttribute)) as EnumMemberAttribute;

            if (memberAttribute is null)
                continue;

            if (memberAttribute.Value?.Equals(value, StringComparison.OrdinalIgnoreCase)??false)
            {
                result = Enum.Parse<TEnum>(name);
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// Gets the enum value from a string honoring the EnumMemberAttribute if present 
    /// </summary>
    public static TEnum? GetEnumValueOrDefault<TEnum>(this string value) where TEnum : struct
    {
        if (TryParseWithMemberName(value, out TEnum result))
            return result;

        return default;
    }
}
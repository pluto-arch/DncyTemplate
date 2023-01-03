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

}
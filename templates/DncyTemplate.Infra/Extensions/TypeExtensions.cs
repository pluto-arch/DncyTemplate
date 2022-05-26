namespace DncyTemplate.Infra.Extensions;

public static class TypeExtensions
{
    public static string GetGenericTypeName(this Type type)
    {
        string typeName = string.Empty;

        if (type.IsGenericType)
        {
            string genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }

        return typeName;
    }

    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetGenericTypeName();
    }
}
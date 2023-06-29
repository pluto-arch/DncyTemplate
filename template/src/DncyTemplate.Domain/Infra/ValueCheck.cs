using System.Diagnostics.CodeAnalysis;

namespace DncyTemplate.Domain.Infra;

[DebuggerStepThrough]
public static class ValueCheck
{
    public static T NotNull<T>(T value, [NotNull] string parameterName)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName);
        }

        return value;
    }

    public static T NotNull<T>(T value, [NotNull] string parameterName, string message)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName, message);
        }

        return value;
    }
}
using System.Runtime.CompilerServices;

namespace ProjectX.Core.Extensions;

public static class ValidationExtensions
{
    public static T ThrowIfNull<T>(this T item, [CallerArgumentExpression("item")] string? paramName = null)
    {
        if (item == null) throw new ArgumentNullException(paramName);

        return item;
    }

    public static string ThrowIfNullOrEmpty(this string text, [CallerArgumentExpression("text")] string? paramName = null)
    {
        if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(paramName);

        return text;
    }
}

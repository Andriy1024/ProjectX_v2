using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ProjectX.Core.Extensions;

public static class ValidationExtensions
{
    public static T ThrowIfNull<T>([NotNull] this T? item, [CallerArgumentExpression("item")] string? paramName = null)
        => item ?? throw new ArgumentNullException(paramName);
    
    public static string ThrowIfNullOrEmpty([NotNull] this string? text, [CallerArgumentExpression("text")] string? paramName = null)
        => string.IsNullOrEmpty(text)
            ? throw new ArgumentNullException(paramName)
            : text;
}
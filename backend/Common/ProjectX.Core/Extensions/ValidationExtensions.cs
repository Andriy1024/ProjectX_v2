using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ProjectX.Core;

public static class ValidationExtensions
{
    [return: NotNull]
    public static T ThrowIfNull<T>([NotNull] this T? item, [CallerArgumentExpression("item")] string? paramName = null)
        => item ?? throw new ArgumentNullException($"{typeof(T).Name}: {paramName}");

    [return: NotNull]
    public static string ThrowIfNullOrEmpty([NotNull] this string? text, [CallerArgumentExpression("text")] string? paramName = null)
        => string.IsNullOrEmpty(text)
            ? throw new ArgumentNullException(paramName)
            : text;
}
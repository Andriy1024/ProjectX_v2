using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ProjectX.Core.Extensions;

public static class ValidationUtility
{
    public static T ThrowIfInvalid<T>([NotNull] this T? obj, [CallerArgumentExpression("item")] string? paramName = null)
    {
        obj.ThrowIfNull(paramName);

        var ctx = new ValidationContext(obj);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(obj, ctx, results, true))
        {
            throw new ValidationException($"{typeof(T).Name}: {string.Join(',', results.Select(r => r.ErrorMessage))}");
        }

        return obj;
    }
}
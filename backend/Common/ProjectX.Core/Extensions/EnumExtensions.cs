using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ProjectX.Core.Extensions;

public static class EnumExtensions
{
    public static string GetDescription([NotNull] this Enum? value) =>
        value.ThrowIfNull().GetType().GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault()?.Description
                ?? value.ToString();
}
using System.ComponentModel;

namespace ProjectX.Core;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value) =>
        value.ThrowIfNull().GetType().GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault()?.Description
                ?? value.ToString();
}
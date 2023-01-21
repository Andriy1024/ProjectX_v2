using System.Reflection;

namespace ProjectX.Core.Enums;

public abstract class StringEnum<TEnum>
    where TEnum : StringEnum<TEnum>
{
    private static readonly Dictionary<string, TEnum> _values = GetAll();

    public string Name { get; }

    protected StringEnum(string name) => Name = name;

    public static TEnum FromName(string name)
    {
        if (_values.TryGetValue(name, out var enumValue))
        {
            return enumValue;
        }

        throw new ArgumentOutOfRangeException($"{typeof(TEnum).FullName} does not contain defenition for {name}");
    }

    public static Dictionary<string, TEnum> GetAll()
    {
        var enumType = typeof(TEnum);

        return enumType
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(f => enumType.IsAssignableFrom(f.FieldType))
            .Select(f => (TEnum)f.GetValue(default)!)
            .ToDictionary(x => x.Name);
    }

    public override int GetHashCode() => Name.GetHashCode();

    public override string ToString() => Name;

    public override bool Equals(object? obj) => Equals(obj as TEnum);

    public bool Equals(TEnum? other) => Name.Equals(other?.Name);
}
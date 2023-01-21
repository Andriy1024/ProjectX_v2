namespace ProjectX.Core;

/// <summary>
/// Base class for constant class that holds the string value.
/// </summary>
public class Enumeration
{
    static readonly object _staticFieldsLock = new object();
    static Dictionary<Type, Dictionary<string, Enumeration>> _staticFields =
        new Dictionary<Type, Dictionary<string, Enumeration>>();

    protected Enumeration(string value)
    {
        this.Value = value;
    }

    /// <summary>
    /// Gets the value that needs to be used when send the value to AWS
    /// </summary>
    public string Value
    {
        get;
        private set;
    }

    public override string ToString()
        => this.Intern().Value;
    
    public string ToString(IFormatProvider provider)
        => this.Intern().Value;
    
    public static implicit operator string(Enumeration value)
        => value == null
            ? null
            : value.Intern().Value;

    /// <summary>
    /// Attempt to find correct-cased constant value using whatever cased value the user
    /// has provided. This is primarily useful for mapping any-cased values from a CLI
    /// tool to the specific casing required by the service, avoiding the need for the
    /// user to (a) remember the specific case and (b) actually type it correctly.
    /// </summary>
    /// <returns>The properly cased service constant matching the value</returns>
    internal Enumeration Intern()
    {
        if (!_staticFields.ContainsKey(this.GetType()))
            LoadFields(this.GetType());

        var map = _staticFields[this.GetType()];
        Enumeration foundValue;
        return map.TryGetValue(this.Value, out foundValue) ? foundValue : this;
    }

    protected static bool HasValue<T>(string value) where T : Enumeration
    {
        if (value == null)
            return false;

        if (!_staticFields.ContainsKey(typeof(T)))
            LoadFields(typeof(T));

        var fields = _staticFields[typeof(T)];
        return fields.ContainsKey(value);
    }

    protected static void Validate<T>(string value) where T : Enumeration
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        if (!_staticFields.ContainsKey(typeof(T)))
            LoadFields(typeof(T));

        var fields = _staticFields[typeof(T)];

        if (!fields.ContainsKey(value))
            throw new ArgumentOutOfRangeException(value);
    }

    public static T FindValue<T>(string value) where T : Enumeration
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        var type = typeof(T);
        if (!_staticFields.ContainsKey(type))
            LoadFields(type);

        var fields = _staticFields[type];
        Enumeration foundValue;
        if (!fields.TryGetValue(value, out foundValue))
        {
            throw new ArgumentOutOfRangeException($"Type: {type.Name}, value: {value}");
        }

        return foundValue as T;
    }

    private static void LoadFields(Type t)
    {
        if (_staticFields.ContainsKey(t))
            return;

        lock (_staticFieldsLock)
        {
            if (_staticFields.ContainsKey(t)) return;

            var map = new Dictionary<string, Enumeration>(StringComparer.OrdinalIgnoreCase);

            foreach (var fieldInfo in t.GetFields())
            {
                if (fieldInfo.IsStatic && fieldInfo.FieldType == t)
                {
                    var cc = fieldInfo.GetValue(null) as Enumeration;
                    map[cc.Value] = cc;
                }
            }

            // create copy of dictionary with new value
            var newDictionary = new Dictionary<Type, Dictionary<string, Enumeration>>(_staticFields);
            newDictionary[t] = map;

            // swap in the new dictionary
            _staticFields = newDictionary;
        }
    }

    public override int GetHashCode()
        => this.Value.GetHashCode();
    
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        // If both are the same instance, return true.
        if (System.Object.ReferenceEquals(this, obj))
            return true;

        var objConstantClass = obj as Enumeration;
        if (this.Equals(objConstantClass))
            return true;

        var objString = obj as string;
        if (objString != null)
            return Equals(objString);

        // obj is of an incompatible type, return false.
        return false;
    }

    public virtual bool Equals(Enumeration obj)
    {
        if ((object)obj == null)
            return false;

        return StringComparer.OrdinalIgnoreCase.Equals(this.Value, obj.Value);
    }

    protected virtual bool Equals(string value)
        => StringComparer.OrdinalIgnoreCase.Equals(this.Value, value);
    
    public static bool operator ==(Enumeration a, Enumeration b)
    {
        if (System.Object.ReferenceEquals(a, b))
        {
            // If both are null, or both are the same instance, return true.
            return true;
        }

        if ((object)a == null)
        {
            // If either is null, return false.
            return false;
        }
        else
        {
            return a.Equals(b);
        }
    }
    
    public static bool operator ==(Enumeration a, string b)
    {
        if ((object)a == null && b == null)
        {
            return true;
        }

        if ((object)a == null)
        {
            // If either is null, return false.
            return false;
        }
        else
        {
            return a.Equals(b);
        }
    }

    public static bool operator !=(Enumeration a, Enumeration b)
        => !(a == b);

    public static bool operator ==(string a, Enumeration b)
        => (b == a);
    
    public static bool operator !=(Enumeration a, string b)
        => !(a == b);
    
    public static bool operator !=(string a, Enumeration b)
        => !(a == b);
}

namespace ProjectX.Observability;

public record TraceCode
{
    private TraceCode(string code) => Code = code;

    public string Code { get; }

    public static TraceCode Success = new("OK");

    public static TraceCode Error = new("ERROR");

    public static TraceCode Unknown = new("UNSET");
}
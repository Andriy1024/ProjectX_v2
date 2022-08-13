using System.Diagnostics;

namespace ProjectX.Core.Observability;

public record TraceCode
{
    private TraceCode(string code) => Code = code;

    public string Code { get; }

    public static TraceCode Success = new(ActivityStatusCode.Ok.ToString().ToUpper());

    public static TraceCode Error = new(ActivityStatusCode.Error.ToString().ToUpper());

    public static TraceCode Unknown = new(ActivityStatusCode.Unset.ToString().ToUpper());
}